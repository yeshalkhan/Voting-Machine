# Voting Machine Interface with Serialization and Database Integration

## Contents and Objectives:
- Design and implement a simple voting machine interface.
- Demonstrate the use of classes, data members, functions, object serialization, ADO.NET for database operations, byte-oriented stream, character-orientedstream, object initializer syntax, params keyword, ToString override, and baseobject class.

## Tools and Technologies:
- Visual Studio (or any preferred IDE for C# development) .Net
= SQL Server 
- C# programming language

## Interface:

### Class Candidate
- Private Int CandidateID:Generate this id using rand function for each newcandid
- Private String Name:Take input Name from Candidate.
- Private String Party:Take input (party name)from candidate.Party-Name must be unique, as castVote() cast the vote on the basis of selected party name.
- Private Int votes:Just an incrementer, increased by one each time voter castethe vote.
- Private int GenerateCandidateID():This function will generate ID using rand function and returns id.
- Public Candidate(string name, string party):Assign properties in parameters , assign id using GenerateCandidateID(),Initialize votes with 0.
- Public int CandidateID:Create just getter of CandidateID
- Public string Name: Create getterand setter for Name.
- Public string Party: Create getter and setter for Party
- Public int Votes: Create just getter for Votes.
- Public void IncrementVotes():Increment number of votes by 1.
- ToString Override:Override the ToString method in the Candidate class toprovide a custom

### Class Voter
- Private string VoterName
- Private String cnic
- Private String selectedPartyName
- Public Voter(string VoterName,string cnic,string selectedPartyName)
- Public String selectedPartyName:Create only getter for selectedPartyName.
- Public bool hasVoted(String cnic):Check that if this voter already caste thevote, then return true else false

### Class VotingMachine
- private List<Candidate> candidates
- public VotingMachine()
- castVote(Candidate c,Voter v): check if this voter has not already castedthevote, if not ,then cast his vote.
- public addVoter() : take voter details as input,store it in your Database as well as in File system.
- public updateVoter(string cnic): update details on the basis of cnic in your database as well as in your file system.
- public displayVoters(): display details of all voters on the screen.
- void displayCandidates(): display all candidates with their names,partyname,votes.
- public void Declarewinner(): display name and details of winner candidate.
- public void InsertCandidate(Candidate c):insert candidate both in file and in database.Implement a function to write candidate data to a text file using StreamWriter.Serialize an instance of the Candidate class to a binary file.
- public void readCandidate(int id):read candidate both from file and db.Implement this function to read and deserialize candidate data from a binary file using FileStream and BinaryFormatter.
- public void updateCandidate(Candidate c , int ID):update candidate both in file and dbonthe basis of ID.
- public void deleteCandidate(int ID):delete candidate both from file and db on the basis of ID.

## Database Schemas

### Candidate Table:
Create a table named Candidates with the following columns:
- CandidateID: An integer column serving as the primary key.
- Name: A string column for the candidate's name.
- Party: A string column for the candidate's party affiliation.
- Votes: An integer column to store the number of votes the candidate has received.

### Voter Table:
Create a table name Voters with following column:
- VoterID: An integer column serving as the primary key.
- VoterName: A string column (nvarchar) to store the name of the voter.
- SelectedPartyName: A string (nvarchar) to store the name of the selected Party Name.

## Menu based interface
Create a menu based program from the admin point of view , who had access toperform all crud operations

------------------------------------Welcome to Online Voting System------------------------------
1. addVoter()
2. updateVoter(string cnic)
3. deleteVoter(string cnic)
4. displayVoters()
5. Castevote()
6. insertCandidate()
7. UpdateCandidate()
8. DisplayCandidates()
9. DeleteCandidates()
10. DeclareWinner()
Enter your choice from 1 to 10: 
