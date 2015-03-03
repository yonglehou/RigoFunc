// Copyright (c) xyting. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace RigoFunc.Utils {
    using System;
    using System.IO;
    using System.Security.Cryptography;

    /// <summary>
    /// Represents an utility provides the calculate hash code functionality.
    /// </summary>
    public static class HashUtil {
        /// <summary>
        /// Computes the hash for the specified file with the given algorithm.
        /// </summary>
        /// <param name="file">The file full path to calculate hash.</param>
        /// <param name="hashAlgorithm">The hash algorithm to use.</param>
        /// <param name="hash">The file hash calculated.</param>
        /// <returns><c>true</c> if file hash has been calculated successfully; otherwise, <c>false</c>.</returns>
        /// <remarks>If the hash algorithm is not specified, MD5 will be used.</remarks>
        public static bool ComputeHash(string file, HashAlgorithm hashAlgorithm, out string hash) {
            hash = string.Empty;

            // check file existence
            if (!File.Exists(file)) {
                return false;
            }

            // check the hash algorithm
            var algorithm = hashAlgorithm;
            if (algorithm == null) {
                algorithm = MD5.Create();
            }

            // try to open the file
            try {
                using (var fs = File.OpenRead(file)) {
                    // be sure it's positioned to the beginning of the stream.
                    fs.Position = 0;

                    byte[] data = algorithm.ComputeHash(fs);

                    hash = BitConverter.ToString(data).Replace("-", "");
                }

                return true;
            }
            catch {
                return false;
            }
        }

        /// <summary>
        /// Computes the hash for the specified file with the MD5 algorithm.
        /// </summary>
        /// <param name="file">The file full path to calculate hash.</param>
        /// <param name="hash">The file hash calculated.</param>
        /// <returns><c>true</c> if file hash has been calculated successfully; otherwise, <c>false</c>.</returns>
        public static bool ComputeHashUsingMD5(string file, out string hash) {
            return ComputeHash(file, MD5.Create(), out hash);
        }

        /// <summary>
        /// Computes the hash for the specified file with the Secure Hash Algorithms SHA-512 algorithm.
        /// </summary>
        /// <param name="file">The file full path to calculate hash.</param>
        /// <param name="hash">The file hash calculated.</param>
        /// <returns><c>true</c> if file hash has been calculated successfully; otherwise, <c>false</c>.</returns>
        public static bool ComputeHashUsingSHA512(string file, out string hash) {
            return ComputeHash(file, new SHA512Managed(), out hash);
        }

        /// <summary>
        /// Computes the hash for the specified data.
        /// </summary>
        /// <param name="data">The data to be computed the hash.</param>
        /// <returns>The hash for the data.</returns>
        public static byte[] ComputeHash(byte[] data) {
            return ComputeHashImpl(data);
        }

        /// <summary>
        /// Verifies the hash for the specified data.
        /// </summary>
        /// <param name="data">The data to be verified.</param>
        /// <param name="hash">The hash that is used to verify the <paramref name="data" />.</param>
        /// <returns><c>true</c> if the hash is matched to the <paramref name="data" />; otherwise, <c>false</c>.</returns>
        public static bool VerifyHash(byte[] data, byte[] hash) {
            byte[] hashForData = ComputeHashImpl(data);

            return CheckEqual(hash, hashForData);
        }


        private static byte[] ComputeHashImpl(byte[] data) {
            if (data == null) {
                return null;
            }

            // compute hash
            var algorithm = MD5.Create();
            byte[] hashBytes = algorithm.ComputeHash(data);
            algorithm.Clear();

            return hashBytes;
        }

        private static bool CheckEqual(byte[] array1, byte[] array2) {
            if (array1 == null && array2 == null) {
                return true;
            }
            else if (array1 == null || array2 == null) {
                return false;
            }
            else if (array1.Length != array2.Length) {
                return false;
            }
            else {
                for (int i = 0; i < array1.Length; i++) {
                    if (array1[i] != array2[i]) {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
