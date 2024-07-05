using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text;
using System.IO;
using System.Linq;
using System.Data.SqlClient;

namespace Assignment1
{
    class VotingMachine
    {
        List<Candidate> candidates;
        public VotingMachine()
        {
      
        }
        public void AddVoter()
        {
            //Input voter details from user

            Console.WriteLine("Enter Voter Details: ");
            Console.Write("Name: ");
            string name = Console.ReadLine();
            Console.Write("CNIC: ");
            string cnic = Console.ReadLine();

            Voter v = new Voter { VoterName = name, Cnic = cnic };

            //Check if voter already exists in the system

            string ConnString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\computer point\\Documents\\VMDatabase.mdf;Integrated Security=True";
            SqlConnection Conn1 = new SqlConnection(ConnString);
            string query = $"SELECT * FROM Voter WHERE VoterID = '{cnic}'";
            SqlCommand cmd1 = new SqlCommand(query, Conn1);
            try
            { Conn1.Open(); }
            catch
            { Console.WriteLine("\nFailed to Connect Database!\n"); }
            SqlDataReader sdr = cmd1.ExecuteReader();

            if (sdr.HasRows)
            {
                Console.WriteLine("\nVoter Already Exists In The System!\n");
                Conn1.Close();
                return;
            }

            Conn1.Close();

            //Store voter details in database

            SqlConnection Conn2 = new SqlConnection(ConnString);
            query = $"INSERT INTO Voter (VoterName, VoterID) VALUES ('{name}', '{cnic}')";
            SqlCommand cmd2 = new SqlCommand(query, Conn2);
            try
            { Conn2.Open(); }
            catch
            { Console.WriteLine("\nFailed to Connect Database!\n"); }
            if (cmd2.ExecuteNonQuery() == 1)
                Console.WriteLine("\nVoter Added Successfully\n");
            else
            {
                Console.WriteLine("\nFailed to Add Voter!\n");
                Conn2.Close();
                return;
            }
            Conn2.Close();

            //Store voter details in text file through serialization

            string JsonString = JsonSerializer.Serialize(v);
            StreamWriter sw = new StreamWriter("Voters.txt", true);
            sw.WriteLine(JsonString);
            sw.Close();
        }

        public void UpdateVoter()
        {
            //Input voter details from user

            Console.Write("Enter CNIC of Voter to Update : ");
            string Cnic = Console.ReadLine();
            Console.WriteLine("\nEnter New Voter Details:");
            Console.Write("Name: Updated ");
            string Name = Console.ReadLine();

            //Update voter's details in database

            string ConnString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\computer point\\Documents\\VMDatabase.mdf;Integrated Security=True";
            SqlConnection Conn = new SqlConnection(ConnString);
            string query = $"UPDATE Voter SET VoterName ='{Name}' WHERE VoterID = '{Cnic}'";
            SqlCommand cmd = new SqlCommand(query, Conn);
            try
            { Conn.Open(); }
            catch 
            { Console.WriteLine("\nFailed to Connect Database!\n"); }
            if (cmd.ExecuteNonQuery() == 1)
                Console.WriteLine("\nVoter Updated Successfully!\n");
            else
            {
                Console.WriteLine("\nNo Such Voter Exists in the System!\n");
                Conn.Close();
                return;
            }
            Conn.Close();

            //Update voter's details in text file

            StreamReader sr = new StreamReader("Voters.txt");
            string JsonString;
            List<string> lines = new List<string>();
            while ((JsonString = sr.ReadLine()) != null)
            {
                Voter v = JsonSerializer.Deserialize<Voter>(JsonString);
                if (v.Cnic == Cnic)
                {
                    v.VoterName = Name;
                    JsonString = JsonSerializer.Serialize(v);
                }
                lines.Add(JsonString);
            }
            sr.Close();
            StreamWriter sw = new StreamWriter("Voters.txt", false);    //overwriting the file
            foreach (string line in lines)
                sw.WriteLine(line);
            sw.Close();
        }
        public void DisplayVoters()
        {
            Console.WriteLine("List of Voters:");
            string ConnString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\computer point\\Documents\\VMDatabase.mdf;Integrated Security=True";
            SqlConnection Conn = new SqlConnection(ConnString);
            string query = "SELECT * FROM Voter";
            SqlCommand cmd = new SqlCommand(query, Conn);
            try
            { Conn.Open(); }
            catch
            { Console.WriteLine("\nFailed to Connect Database!\n"); }
            SqlDataReader sdr = cmd.ExecuteReader();
            int Count = 1;
            while(sdr.Read())
                Console.WriteLine($"{Count++}. {sdr[1].ToString()} - CNIC: {sdr[0].ToString()}");
            Conn.Close();
        }
        public void CasteVote()
        {
            //Input voter and candidate details from user

            Console.Write("Enter CNIC of Voter to Caste Vote: ");
            string Cnic = Console.ReadLine();
            Console.Write("Enter ID of Candidate to Caste Vote: ");
            int ID = int.Parse(Console.ReadLine());

            //Check if voter exists in the system

            StreamReader sr3 = new StreamReader("Voters.txt");
            bool flag = false;
            string JsonString;
            while ((JsonString = sr3.ReadLine()) != null)
            {
                Voter v = JsonSerializer.Deserialize<Voter>(JsonString);
                if (v.Cnic == Cnic)
                {
                    flag = true;
                    JsonString = JsonSerializer.Serialize(v);
                    if (v.hasVoted())
                    {
                        Console.WriteLine("\nVoter Has Already Voted!\n");
                        sr3.Close();
                        return;
                    }
                    break;
                }
            }
            sr3.Close();
            if (flag == false)
            {
                Console.WriteLine("\nNo Such Voter Exists in the System!\n\n");
                return;
            }

            //Check if candidate exists in the system and update candidate's votes in database

            string ConnString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\computer point\\Documents\\VMDatabase.mdf;Integrated Security=True";
            SqlConnection Conn3 = new SqlConnection(ConnString);
            string query = $"UPDATE Candidate SET Votes = Votes+1 WHERE CandidateID = '{ID}'";
            SqlCommand cmd3 = new SqlCommand(query, Conn3);
            Conn3.Open();
            if (cmd3.ExecuteNonQuery() != 1)
            {
                Console.WriteLine("\nNo Such Candidate Exists in the System!\n\n");
                Conn3.Close();
                return;
            }
            Conn3.Close();

            //Update candidate's votes in the file

            StreamReader sr = new StreamReader("Candidates.txt");
            string Party = null;
            List<string> lines = new List<string>();
            while ((JsonString = sr.ReadLine()) != null)
            {
                Candidate c = JsonSerializer.Deserialize<Candidate>(JsonString);
                if (c.CandidateID == ID)
                {
                    c.IncrementVotes();
                    Party = c.Party;
                    JsonString = JsonSerializer.Serialize(c);
                }
                lines.Add(JsonString);
            }
            sr.Close();
            StreamWriter sw = new StreamWriter("Candidates.txt", false);    //overwriting the file
            foreach (string line in lines)
                sw.WriteLine(line);
            sw.Close();

            //Update voter's selected party name in database

            SqlConnection Conn2 = new SqlConnection(ConnString);
            query = $"UPDATE Voter SET SelectedPartyName = '{Party}' WHERE VoterID = '{Cnic}'";
            SqlCommand cmd2 = new SqlCommand(query, Conn2);
            Conn2.Open();
            cmd2.ExecuteNonQuery();
            Conn2.Close();

            //Update voter's selected party name in the file

            StreamReader sr2 = new StreamReader("Voters.txt");
            lines.Clear();
            while ((JsonString = sr2.ReadLine()) != null)
            {
                Voter v = JsonSerializer.Deserialize<Voter>(JsonString);
                if (v.Cnic == Cnic)
                {
                    v.SelectedPartyName = Party;
                    JsonString = JsonSerializer.Serialize(v);
                }
                lines.Add(JsonString);
            }
            sr2.Close();
            StreamWriter sw2 = new StreamWriter("Voters.txt", false);    //overwriting the file
            foreach (string line in lines)
                sw2.WriteLine(line);
            sw2.Close();

            Console.WriteLine("\nVote Casted Successfully!\n");
        }
        public void InsertCandidate()
        {
            //Input candidate details from user

            Console.WriteLine("Enter Candidate Details: ");
            Console.Write("Name: ");
            string name = Console.ReadLine();
            Console.Write("Party: ");
            string party = Console.ReadLine();

            //Check if candidate's party is unique

            string ConnString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\computer point\\Documents\\VMDatabase.mdf;Integrated Security=True";
            SqlConnection Conn1 = new SqlConnection(ConnString);
            string query = $"SELECT * FROM Candidate WHERE Party = '{party}'";
            SqlCommand cmd1 = new SqlCommand(query, Conn1);
            try
            { Conn1.Open(); }
            catch
            { Console.WriteLine("\nFailed to Connect Database!\n"); }
            SqlDataReader sdr = cmd1.ExecuteReader();

            if (sdr.HasRows)
            {
                Console.WriteLine("\nThis Party Already Exists in the System!\n");
                Conn1.Close();
                return;
            }

            Conn1.Close();

            //Insert candidate details in database

            Candidate c = new Candidate(name, party);
            SqlConnection Conn2 = new SqlConnection(ConnString);
            query = $"INSERT INTO Candidate (CandidateID,Name,Party) VALUES ({c.CandidateID},'{name}','{party}')";
            SqlCommand cmd2 = new SqlCommand(query, Conn2);
            try
            { Conn2.Open(); }
            catch
            { Console.WriteLine("\nFailed to Connect Database!\n"); }
            if (cmd2.ExecuteNonQuery() == 1)
                Console.WriteLine("\nCandidate Inserted Successfully\n");
            else
            {
                Console.WriteLine("\nFailed to Insert Candidate!\n");
                Conn2.Close();
                return;
            }
            Conn2.Close();

            //Store candidate details in text file through serialization

            string JsonString = JsonSerializer.Serialize(c);
            StreamWriter sw = new StreamWriter("Candidates.txt", true);
            sw.WriteLine(JsonString);
            sw.Close();
        }
        public void UpdateCandidate()
        {
            //Input candidate details from user

            Console.Write("Enter ID of Candidate to Update : ");
            int ID = int.Parse(Console.ReadLine());
            Console.WriteLine("\nEnter New Candidate Details:");
            Console.Write("Name: Updated ");
            string Name = Console.ReadLine();
            Console.Write("Party: Updated ");
            string Party = Console.ReadLine();

            //Update candidate's details in database

            string ConnString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\computer point\\Documents\\VMDatabase.mdf;Integrated Security=True";
            SqlConnection Conn = new SqlConnection(ConnString);
            string query = $"UPDATE Candidate SET Name ='{Name}' WHERE CandidateID = {ID}";
            SqlCommand cmd = new SqlCommand(query, Conn);
            try
            { Conn.Open(); }
            catch
            { Console.WriteLine("\nFailed to Connect Database!\n"); }
            //Update name
            if (cmd.ExecuteNonQuery() != 1)
            {
                Console.WriteLine("\nNo Such Candidate Exists in the System!\n");
                Conn.Close();
                return;
            }
            //Update party
            query= $"UPDATE Candidate SET Party ='{Party}' WHERE CandidateID = {ID}";
            cmd = new SqlCommand(query, Conn);
            try
            { cmd.ExecuteNonQuery(); }
            catch
            {
                Console.WriteLine("\nThis Party Already Exists in the System!\n");
            }
            Console.WriteLine("\nCandidate Updated Successfully!\n");
            Conn.Close();

            //Update candidate's details in text file

            StreamReader sr = new StreamReader("Candidates.txt");
            string JsonString;
            List<string> lines = new List<string>();
            while ((JsonString = sr.ReadLine()) != null)
            {
                Candidate c = JsonSerializer.Deserialize<Candidate>(JsonString);
                if (c.CandidateID == ID)
                {
                    c.Name = Name;
                    c.Party = Party;
                    JsonString = JsonSerializer.Serialize(c);
                }
                lines.Add(JsonString);
            }
            sr.Close();
            StreamWriter sw = new StreamWriter("Candidates.txt", false);    //overwriting the file
            foreach (string line in lines)
                sw.WriteLine(line);
            sw.Close();
        }
        public void DisplayCandidates()
        {
            Console.WriteLine("List of Candidates:");
            string ConnString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\computer point\\Documents\\VMDatabase.mdf;Integrated Security=True";
            SqlConnection Conn = new SqlConnection(ConnString);
            string query = "SELECT * FROM Candidate";
            SqlCommand cmd = new SqlCommand(query, Conn);
            try
            { Conn.Open(); }
            catch
            { Console.WriteLine("\nFailed to Connect Database!\n"); }
            SqlDataReader sdr = cmd.ExecuteReader();
            Console.WriteLine("\nID        Name         Party         Votes\n");
            while (sdr.Read())
                Console.WriteLine($"{sdr[0].ToString()}    {sdr[1].ToString()}   {sdr[2].ToString()}    {sdr[3].ToString()}");
            Conn.Close();
        }
        public void ReadCandidate()
        {
            Console.Write("Enter Candidate Id to Read: ");
            int id = int.Parse(Console.ReadLine());

            //Reading data from file

            Console.WriteLine("\nReading Candidate Details From File...");
            StreamReader sd = new StreamReader("Candidates.txt");
            string JsonString;
            bool flag = false;
            while ((JsonString = sd.ReadLine()) != null)
            {
                Candidate c = JsonSerializer.Deserialize<Candidate>(JsonString);
                if (c.CandidateID == id)
                {
                    Console.WriteLine($"CandidateID: {c.CandidateID}");
                    Console.WriteLine($"Name: {c.Name}");
                    Console.WriteLine($"Party: {c.Party}");
                    Console.WriteLine($"Votes: {c.Votes}\n");
                    flag = true;
                    break;
                }
            }
            sd.Close();
            if (flag == false)
            {
                Console.WriteLine("\nCandidate Doesn't Exist in the System!\n");
                return;
            }

            //Reading data from database

            Console.WriteLine("Reading Candidate Details From database...");
            string ConnString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\computer point\\Documents\\VMDatabase.mdf;Integrated Security=True";
            SqlConnection Conn = new SqlConnection(ConnString);
            string query = $"SELECT * FROM Candidate WHERE CandidateID = '{id}'";
            SqlCommand cmd = new SqlCommand(query, Conn);
            try
            { Conn.Open(); }
            catch
            { Console.WriteLine("\nFailed to Connect Database!\n"); }
            SqlDataReader sdr = cmd.ExecuteReader();

            if (sdr.HasRows)
            {
                sdr.Read();
                Console.WriteLine($"CandidateID: {sdr[0].ToString()}");
                Console.WriteLine($"Name: {sdr[1].ToString()}");
                Console.WriteLine($"Party: {sdr[2].ToString()}");
                Console.WriteLine($"Votes: {sdr[3].ToString()}\n");
            }

            Conn.Close();
        }
        public void DeleteCandidate()
        {
            Console.Write("Enter ID of Candidate to Delete : ");
            int ID = int.Parse(Console.ReadLine());

            //Delete candidate from database

            string ConnString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\computer point\\Documents\\VMDatabase.mdf;Integrated Security=True";
            SqlConnection Conn = new SqlConnection(ConnString);
            string query = $"DELETE FROM Candidate WHERE CandidateID = {ID}";
            SqlCommand cmd = new SqlCommand(query, Conn);
            try
            { Conn.Open(); }
            catch 
            { Console.WriteLine("\nFailed to Connect Database!\n"); }
            if (cmd.ExecuteNonQuery() == 1)
                Console.WriteLine("\nCandidate Deleted Succesfully!\n");
            else
            {
                Console.WriteLine("\nNo Such Candidate Exists in the System!\n");
                Conn.Close();
                return;
            }
            Conn.Close();

            //Delete candidate from file

            StreamReader sd = new StreamReader("Candidates.txt");
            List<string> lines = new List<string>();
            string JsonString;
            while ((JsonString = sd.ReadLine()) != null)
            {
                Candidate c = JsonSerializer.Deserialize<Candidate>(JsonString);
                if (c.CandidateID != ID)
                    lines.Add(JsonString);
            }
            sd.Close();
            StreamWriter sw = new StreamWriter("Candidates.txt", false);    //overwriting the file
            foreach (string line in lines)
                sw.WriteLine(line);
            sw.Close();
        }
        public void DeclareWinner()
        {
            candidates = new List<Candidate>();
            string JsonString;
            StreamReader sd = new StreamReader("Candidates.txt");
            while ((JsonString = sd.ReadLine()) != null)
                candidates.Add(JsonSerializer.Deserialize<Candidate>(JsonString));
            Candidate winner = candidates.OrderByDescending(c => c.Votes).FirstOrDefault();
            Console.WriteLine("Winner : " + winner.Name + "\n");
        }
    }
}
