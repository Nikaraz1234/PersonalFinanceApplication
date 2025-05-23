﻿using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using PersonalFinanceApplication.Interfaces;

namespace PersonalFinanceApplication.PasswordHasher
{


    public class PasswordHasher : IPasswordHasher
    {

        private const int SaltSize = 16; 
        private const int HashSize = 32; 
        private const int Iterations = 10000; 

        public string HashPassword(string password)
        {

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException(nameof(password), "Password cannot be null or empty");
            }

            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }


            byte[] hash = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: Iterations,
                numBytesRequested: HashSize);


            byte[] hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);


            return Convert.ToBase64String(hashBytes);
        }

        public bool VerifyPassword(string storedHash, string providedPassword)
        {

            byte[] hashBytes = Convert.FromBase64String(storedHash);


            byte[] salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            byte[] storedSubHash = new byte[HashSize];
            Array.Copy(hashBytes, SaltSize, storedSubHash, 0, HashSize);


            byte[] computedHash = KeyDerivation.Pbkdf2(
                password: providedPassword,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: Iterations,
                numBytesRequested: HashSize);

            return CryptographicOperations.FixedTimeEquals(computedHash, storedSubHash);
        }
    }
}
