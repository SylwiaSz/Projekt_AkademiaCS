namespace WpfApplication1
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    public class Note : CalendarPart
    {
        public string noteText;

        private bool msgEncripted;

        public Note(string noteText)
        {
            this.noteText = noteText;
            var encriptButton = new Button
                                    {
                                        Name = "encriptButton",
                                        Content = "ENCRIPT",
                                        HorizontalAlignment = HorizontalAlignment.Left,
                                        VerticalAlignment = VerticalAlignment.Center,
                                        Foreground = Brushes.DarkTurquoise,
                                        Background = Brushes.DarkSlateGray,
                                        BorderBrush = Brushes.DarkTurquoise,
                                        Margin = new Thickness(10, 0, 10, 0),
                                        Width = 70,
                                        Height = 30
                                    };
            encriptButton.Click += (s, e) => this.encript();

            var showButton = new Button
                                 {
                                     Name = "showButton",
                                     Content = "SHOW",
                                     HorizontalAlignment = HorizontalAlignment.Left,
                                     VerticalAlignment = VerticalAlignment.Center,
                                     Foreground = Brushes.DarkTurquoise,
                                     Background = Brushes.DarkSlateGray,
                                     BorderBrush = Brushes.DarkTurquoise,
                                     Margin = new Thickness(
                                         encriptButton.Margin.Left + encriptButton.Width + 10,
                                         0,
                                         10,
                                         0),
                                     Width = 50,
                                     Height = 30
                                 };

            showButton.Click += (s, e) => this.show();

            this.Children.Add(showButton);
            this.Children.Add(encriptButton);
        }

        public override void EditMethod(CalendarPart part)
        {
            var editedNote = (Note)part;
            this.noteText = editedNote.noteText;
            this.Children.OfType<TextBox>().First(x => x.Name.Contains("TextBox")).Text = this.noteText;
        }

        private void decryptMsg(string password)
        {
            try
            {
                MessageBox.Show(Cipher.Decrypt(this.noteText, password));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Password incorrect");
            }
        }

        private void encript()
        {
            var passwordBox = new PasswordWindow();
            passwordBox.SavePasswordEvent += this.encryptMsg;
            passwordBox.Show();
            this.msgEncripted = true;
            this.Children.OfType<Button>().First(x => x.Name.Equals("encriptButton")).IsEnabled = false;
        }

        private void encryptMsg(string password)
        {
            this.noteText = Cipher.Encrypt(this.noteText, password);
        }

        private void show()
        {
            if (this.msgEncripted)
            {
                var passwordBox = new PasswordWindow();
                passwordBox.SavePasswordEvent += this.decryptMsg;
                passwordBox.Show();
            }
            else
            {
                MessageBox.Show(this.noteText);
            }
        }

        public static class Cipher
        {
            /// <summary>
            ///     Decrypt a string.
            /// </summary>
            /// <param name="encryptedText">String to be decrypted</param>
            /// <param name="password">Password used during encryption</param>
            /// <exception cref="FormatException"></exception>
            public static string Decrypt(string encryptedText, string password)
            {
                if (encryptedText == null) return null;

                if (password == null) password = string.Empty;

                // Get the bytes of the string
                var bytesToBeDecrypted = Convert.FromBase64String(encryptedText);
                var passwordBytes = Encoding.UTF8.GetBytes(password);

                passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

                var bytesDecrypted = Decrypt(bytesToBeDecrypted, passwordBytes);

                return Encoding.UTF8.GetString(bytesDecrypted);
            }

            /// <summary>
            ///     Encrypt a string.
            /// </summary>
            /// <param name="plainText">String to be encrypted</param>
            /// <param name="password">Password</param>
            public static string Encrypt(string plainText, string password)
            {
                if (plainText == null) return null;

                if (password == null) password = string.Empty;

                // Get the bytes of the string
                var bytesToBeEncrypted = Encoding.UTF8.GetBytes(plainText);
                var passwordBytes = Encoding.UTF8.GetBytes(password);

                // Hash the password with SHA256
                passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

                var bytesEncrypted = Encrypt(bytesToBeEncrypted, passwordBytes);

                return Convert.ToBase64String(bytesEncrypted);
            }

            private static byte[] Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
            {
                byte[] decryptedBytes = null;

                // Set your salt here, change it to meet your flavor:
                // The salt bytes must be at least 8 bytes.
                var saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

                using (var ms = new MemoryStream())
                {
                    using (var AES = new RijndaelManaged())
                    {
                        var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);

                        AES.KeySize = 256;
                        AES.BlockSize = 128;
                        AES.Key = key.GetBytes(AES.KeySize / 8);
                        AES.IV = key.GetBytes(AES.BlockSize / 8);
                        AES.Mode = CipherMode.CBC;

                        using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                            cs.Close();
                        }

                        decryptedBytes = ms.ToArray();
                    }
                }

                return decryptedBytes;
            }

            private static byte[] Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
            {
                byte[] encryptedBytes = null;

                // Set your salt here, change it to meet your flavor:
                // The salt bytes must be at least 8 bytes.
                var saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

                using (var ms = new MemoryStream())
                {
                    using (var AES = new RijndaelManaged())
                    {
                        var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);

                        AES.KeySize = 256;
                        AES.BlockSize = 128;
                        AES.Key = key.GetBytes(AES.KeySize / 8);
                        AES.IV = key.GetBytes(AES.BlockSize / 8);

                        AES.Mode = CipherMode.CBC;

                        using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                            cs.Close();
                        }

                        encryptedBytes = ms.ToArray();
                    }
                }

                return encryptedBytes;
            }
        }
    }
}