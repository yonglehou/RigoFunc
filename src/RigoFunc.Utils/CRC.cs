﻿// Copyright (c) xyting. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace RigoFunc.Utils {
    using System.Security.Cryptography;

    public class Crc32 : HashAlgorithm {
        public const uint DefaultPolynomial = 0xedb88320;
        public const uint DefaultSeed = 0xffffffff;

        private uint hash;
        private uint seed;
        private uint[] table;
        private static uint[] defaultTable;

        #region Public Members

        public Crc32() {
            table = InitializeTable(DefaultPolynomial);
            seed = DefaultSeed;
            Initialize();
        }

        public Crc32(uint polynomial, uint seed) {
            table = InitializeTable(polynomial);
            this.seed = seed;
            Initialize();
        }

        public static uint Compute(byte[] buffer) {
            return ~CalculateHash(InitializeTable(DefaultPolynomial), DefaultSeed, buffer, 0, buffer.Length);
        }

        public static uint Compute(uint seed, byte[] buffer) {
            return ~CalculateHash(InitializeTable(DefaultPolynomial), seed, buffer, 0, buffer.Length);
        }

        public static uint Compute(uint polynomial, uint seed, byte[] buffer) {
            return ~CalculateHash(InitializeTable(polynomial), seed, buffer, 0, buffer.Length);
        }

        #endregion

        #region Overriden Members

        public override void Initialize() {
            hash = seed;
        }

        protected override void HashCore(byte[] buffer, int start, int length) {
            hash = CalculateHash(table, hash, buffer, start, length);
        }

        protected override byte[] HashFinal() {
            byte[] hashBuffer = UInt32ToBigEndianBytes(~hash);
            this.HashValue = hashBuffer;
            return hashBuffer;
        }

        public override int HashSize {
            get { return 32; }
        }

        #endregion

        #region Helper Methods

        private static uint[] InitializeTable(uint polynomial) {
            if (polynomial == DefaultPolynomial && defaultTable != null)
                return defaultTable;

            uint[] createTable = new uint[256];
            for (int i = 0; i < 256; i++) {
                uint entry = (uint)i;
                for (int j = 0; j < 8; j++)
                    if ((entry & 1) == 1)
                        entry = (entry >> 1) ^ polynomial;
                    else
                        entry = entry >> 1;
                createTable[i] = entry;
            }

            if (polynomial == DefaultPolynomial)
                defaultTable = createTable;

            return createTable;
        }

        private static uint CalculateHash(uint[] table, uint seed, byte[] buffer, int start, int size) {
            uint crc = seed;
            for (int i = start; i < size; i++)
                unchecked {
                    crc = (crc >> 8) ^ table[buffer[i] ^ crc & 0xff];
                }
            return crc;
        }

        private byte[] UInt32ToBigEndianBytes(uint x) {
            return new byte[] {
                (byte)((x >> 24) & 0xff),
                (byte)((x >> 16) & 0xff),
                (byte)((x >> 8) & 0xff),
                (byte)(x & 0xff)
            };
        }

        #endregion
    }
}
