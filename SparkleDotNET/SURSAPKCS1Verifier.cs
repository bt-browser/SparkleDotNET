﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace SparkleDotNET {
    class SURSAPKCS1Verifier {


        public static bool ValidatePathWithEncodedRSAPKCS1SignatureAndPublicRSAKey(string path, string base64Signature, string publicKey) {

            try {

                byte[] signature = Convert.FromBase64String(base64Signature);
                byte[] data = File.ReadAllBytes(path);
                SHA256CryptoServiceProvider cryptoTransformSHA256 = new SHA256CryptoServiceProvider();
                byte[] sha256Hash = cryptoTransformSHA256.ComputeHash(data);
                string cleanKey = "";

                string[] lines = publicKey.Split(new char[] {'\n', '\r'});

                foreach (string line in lines) {
                        cleanKey += line.Trim();
                }

                byte[] publicKeyData = Convert.FromBase64String(cleanKey);

                RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
                provider.ImportCspBlob(publicKeyData);
                RSAPKCS1SignatureDeformatter formatter = new RSAPKCS1SignatureDeformatter(provider);
                formatter.SetHashAlgorithm("SHA256");
                return formatter.VerifySignature(sha256Hash, signature);

            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return false;
            }

        }
    }
}
