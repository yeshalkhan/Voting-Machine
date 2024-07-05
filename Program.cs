using System;
using System.Data.SqlClient;

namespace Assignment1
{
    class Program
    {
        static void Main(string[] args)
        {
            VotingMachine VM = new VotingMachine();
            Console.WriteLine("------------------------------------Welcome to Online Voting System------------------------------\n");
            Console.WriteLine("1. Add Voter");
            Console.WriteLine("2. Update Voter");
            Console.WriteLine("3. Display Voters");
            Console.WriteLine("4. Caste Vote");
            Console.WriteLine("5. Insert Candidate");
            Console.WriteLine("6. Update Candidate");
            Console.WriteLine("7. Display Candidates");
            Console.WriteLine("8. Read Candidate");
            Console.WriteLine("9. Delete Candidate");
            Console.WriteLine("10. Declare Winner");

            string choice;
            while (true)
            {
                Console.Write("\nEnter your choice from 1 to 10: ");
                choice = Console.ReadLine();
                if (choice == "1")
                {
                    Console.WriteLine("---------------------------------------------");
                    Console.WriteLine("1. Add Voter");
                    Console.WriteLine("---------------------------------------------");
                    VM.AddVoter();
                    Console.WriteLine("---------------------------------------------");
                }
                else if (choice == "2")
                {
                    Console.WriteLine("---------------------------------------------");
                    Console.WriteLine("2. Update Voter");
                    Console.WriteLine("---------------------------------------------");
                    VM.UpdateVoter();
                    Console.WriteLine("---------------------------------------------");
                }
                else if (choice == "3")
                {
                    Console.WriteLine("---------------------------------------------");
                    Console.WriteLine("3. Display Voters");
                    Console.WriteLine("---------------------------------------------");
                    VM.DisplayVoters();
                    Console.WriteLine("---------------------------------------------");
                }
                else if (choice == "4")
                {
                    Console.WriteLine("---------------------------------------------");
                    Console.WriteLine("4. Caste Vote");
                    Console.WriteLine("---------------------------------------------");
                    VM.CasteVote();
                    Console.WriteLine("---------------------------------------------");
                }
                else if (choice == "5")
                {
                    Console.WriteLine("---------------------------------------------");
                    Console.WriteLine("5. Insert Candidate");
                    Console.WriteLine("---------------------------------------------");
                    VM.InsertCandidate();
                    Console.WriteLine("---------------------------------------------");
                }
                else if (choice == "6")
                {
                    Console.WriteLine("---------------------------------------------");
                    Console.WriteLine("6. Update Candidate");
                    Console.WriteLine("---------------------------------------------");
                    VM.UpdateCandidate();
                    Console.WriteLine("---------------------------------------------");
                }
                else if (choice == "7")
                {
                    Console.WriteLine("---------------------------------------------");
                    Console.WriteLine("7. Display Candidates");
                    Console.WriteLine("---------------------------------------------");
                    VM.DisplayCandidates();
                    Console.WriteLine("---------------------------------------------");
                }
                else if (choice == "8")
                {
                    Console.WriteLine("---------------------------------------------");
                    Console.WriteLine("8. Read Candidate");
                    Console.WriteLine("---------------------------------------------");
                    VM.ReadCandidate();
                    Console.WriteLine("---------------------------------------------");
                }
                else if (choice == "9")
                {
                    Console.WriteLine("---------------------------------------------");
                    Console.WriteLine("9. Delete Candidate");
                    Console.WriteLine("---------------------------------------------");
                    VM.DeleteCandidate();
                    Console.WriteLine("---------------------------------------------");
                }
                else if(choice=="10")
                {
                    Console.WriteLine("---------------------------------------------");
                    Console.WriteLine("10. Declare Winner");
                    Console.WriteLine("---------------------------------------------");
                    VM.DeclareWinner();
                    Console.WriteLine("---------------------------------------------");
                }
                else
                {
                    Console.WriteLine("Invalid Choice");
                    break;
                }
            }
            Console.WriteLine("\nWait...");
        }

    }
}
