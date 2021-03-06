﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventArgsLibrary;

namespace ConsoleFormat
{
    public class ConsolePrint
    {
        #region Attributes
        private static long hex_sender_index = 0;
        private static long hex_receiver_index = 0;

        private static byte msgFunctionMSB;
        private static byte msgFunctionLSB;
        private static byte msgPayloadLenghtMSB;
        private static byte msgPayloadLenghtLSB;
        #endregion

        #region Main Methods
        static public void OnPrintEncodedMessage(object sender, MessageEncodedArgs e)
        {
            ResetConsoleCursorAndConsoleColor();
            msgFunctionMSB = e.Msg[0];
            msgFunctionLSB = e.Msg[1];
            msgPayloadLenghtMSB = e.Msg[2];
            msgPayloadLenghtLSB = e.Msg[3];
            Console.Write("[Encoded : " + hex_sender_index++ + "] : ");
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.Write("0x" + msgFunctionMSB.ToString("2X") + " " + "0x" + msgFunctionLSB.ToString("2X") + " ");
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Write("0x" + msgPayloadLenghtMSB.ToString("2X") + " 0x" + msgPayloadLenghtLSB.ToString("2X") + " ");
            Console.BackgroundColor = ConsoleColor.White;
            for (int pos = 4, max = e.Msg.Length - 1; pos < max; pos++)
                Console.Write("0x" + e.Msg[pos].ToString("2x") + " ");
            Console.BackgroundColor = ConsoleColor.Green;
            Console.Write("0x" + e.Msg[e.Msg.Length].ToString("2x"));
        }

        static public void OnPrintDecodedMessage(object sender, MessageDecodedArgs e)
        {
            ResetConsoleCursorAndConsoleColor();
            ConvertMessageToByte(e.MsgFunction, e.MsgPayloadLength);
            Console.Write("[Decoded : " + hex_receiver_index++ + "] : ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("0x" + msgFunctionMSB.ToString("2X") + " " + "0x" + msgFunctionLSB.ToString("2X") + " ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("0x" + msgPayloadLenghtMSB.ToString("2X") + " 0x" + msgPayloadLenghtLSB.ToString("2X") + " ");
            Console.ForegroundColor = ConsoleColor.White;
            for (int poss = 0, max = e.MsgPayload.Length - 1; poss < max; poss++)
                Console.Write("0x" + e.MsgPayload[poss].ToString("2x") + " ");
            if (e.ChecksumCorrect)
                Console.ForegroundColor = ConsoleColor.Green;
            else
                Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("0x" + e.MsgPayload[e.MsgPayload.Length].ToString("2x"));
        }

        static public void OnPrintEvent(object sender, StringEventArgs msg)
        {
            ResetConsoleCursorAndConsoleColor();
            Console.Write("[" + "] Event : " + msg);
        }
        #endregion

        #region Error Messages
        static public void OnPrintCorruptedMessage(object sender, StringEventArgs errorMsg)
        {
            // When message is corrupted
            ResetConsoleCursorAndConsoleColor();
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write("/!\\ WARNING : " + errorMsg + "/!\\");
        }

        static public void OnPrintErrorOfCoding(string errorMsg)
        {
            // The sender have make a mistake 
            ResetConsoleCursorAndConsoleColor();
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.Write("/!\\ WARNING : " + errorMsg + "/!\\");
        }
        #endregion

        #region Sub Method
        static private void ConvertMessageToByte(ushort msgFunction, ushort msgPayloadLenght)
        {
            msgFunctionMSB = (byte)(msgFunction >> 0);
            msgFunctionLSB = (byte)(msgFunction >> 8);
            msgPayloadLenghtMSB = (byte)(msgPayloadLenght >> 0);
            msgPayloadLenghtLSB = (byte)(msgPayloadLenght >> 8);
        }

        static public void ResetConsoleCursorAndConsoleColor()
        {
            if (Console.CursorLeft != 0)
                Console.WriteLine();
            Console.ResetColor();
        }
        #endregion
    }
}