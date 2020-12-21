using Dapper;
using GMS___Model;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace GMS___Data_Access_Layer
{
    public class EventCharacterAccess : IEventCharacterAccess
    {
        private readonly Logger log = LogManager.GetCurrentClassLogger();
        public bool InsertEventCharacter(EventCharacter eventParticipant)
        {

            if (!DBConnection.IsConnectionAvailable())
            {
                log.Error(exception: new TimeoutException(), "No connection to either the internet or the database available.");
                return false;
            }

            // Either join participant list or the waiting list by checking if participant list reached the maximum amout of sign-ups.
            using (IDbConnection conn = DBConnection.GetConnection())
            {
                try
                {
                    if (!IsParticipantListFull(eventParticipant.EventID))
                    {
                        log.Info("Inserting character: @characterName into the participant list for event ID: @eventID", eventParticipant.CharacterName, eventParticipant.EventID);
                        int rowsAffected = conn.Execute(@"INSERT INTO EventCharacter (eventID, characterName, characterRole, signUpDateTime) " +
                        "VALUES (@EventID, @CharacterName, @CharacterRole, @SignUpDateTime)", eventParticipant);

                        if (rowsAffected > 0)
                        {
                            log.Info("Successfully inserted character: @characterName into the participant list for the event: @eventID", eventParticipant.CharacterName, eventParticipant.EventID);
                            return true;
                        }
                        log.Error(new Exception(), "Unable to insert character: @characterName into the participant list for event ID: @eventID", eventParticipant.CharacterName, eventParticipant.EventID);
                        return false;
                    } else
                    {
                        log.Info("Inserting character: @characterName into the waiting list for event ID: @eventID", eventParticipant.CharacterName, eventParticipant.EventID);
                        int rowsAffected = conn.Execute(@"INSERT INTO EventCharacterWaitingList (eventID, characterName, characterRole, signUpDateTime) " +
                        "VALUES (@EventID, @CharacterName, @CharacterRole, @SignUpDateTime)", eventParticipant);

                        if (rowsAffected > 0)
                        {
                            log.Info("Successfully inserted character: @characterName into the waiting list for the event: @eventID", eventParticipant.CharacterName, eventParticipant.EventID);
                            return true;
                        }
                        log.Error(new Exception(), "Unable to insert character: @characterName into the waiting list for event ID: @eventID", eventParticipant.CharacterName, eventParticipant.EventID);
                        return false;
                    }
                } catch (SqlException ex)
                {
                    log.Trace("SQLException encountered while inserting the character into the event list.");
                    log.Error(ex, "Unable to insert character into the event list.");
                    return false;
                }
            }
        }

        public bool ContainsEntry(int eventID, string characterName)
        {

            if (!DBConnection.IsConnectionAvailable())
            {
                log.Error(exception: new TimeoutException(), "No connection to either the internet or the database available.");
                return false;
            }

            using (IDbConnection conn = DBConnection.GetConnection())
            {
                try
                {
                    log.Info("Checking if character: @characterName is on the list for event ID: @eventID", characterName, eventID);
                    int entries = conn.ExecuteScalar<int>(@"SELECT COUNT(*) FROM EventCharacter WHERE eventID = @EventID AND characterName = @CharacterName", new { EventID = eventID, CharacterName = characterName });
                    log.Info("Retrieved participation entry for character: @characterName for event ID: @eventID", characterName, eventID);
                    if (entries == 1)
                    {
                        return true;
                    }
                    return false;
                } catch (SqlException ex)
                {
                    log.Trace("SQLException encountered while inserting the character into the event list.");
                    log.Error(ex, "Unable to insert character into the event list.");
                    return false;
                }
            }
        }

        public bool DeleteEventCharacterByEventIDAndCharacterName(int eventID, string characterName)
        {

            if (!DBConnection.IsConnectionAvailable())
            {
                log.Error(exception: new TimeoutException(), "No connection to either the internet or the database available.");
                return false;
            }

            using (IDbConnection conn = DBConnection.GetConnection())
            {
                try
                {
                    int rowsAffected;

                    if (IsParticipantListFull(eventID))
                    {
                        return MoveCharacterFromWaitingListToParticipantList(eventID, characterName);

                    } else
                    {
                        log.Info("Deleting character: @characterName from the list for event ID: @eventID", characterName, eventID);
                        rowsAffected = conn.Execute(@"DELETE FROM EventCharacter WHERE eventID = @EventID AND characterName = @CharacterName", new { EventID = eventID, CharacterName = characterName });

                        if (rowsAffected > 0)
                        {
                            log.Info("Successfully deleted character: @characterName from the list for event ID: @eventID", characterName, eventID);
                            return true;
                        }
                        log.Error(new Exception(), "Unable to delete the character from the event list");
                        return false;
                    }
                } catch (SqlException ex)
                {
                    log.Trace("SQLException encountered while deleting the character from the event list.");
                    log.Error(ex, "Unable to delete the character from the event list.");
                    return false;
                }
            }
        }

        public bool IsParticipantListFull(int eventID)
        {

            if (!DBConnection.IsConnectionAvailable())
            {
                log.Error(exception: new TimeoutException(), "No connection to either the internet or the database available.");
                return false;
            }

            using (IDbConnection conn = DBConnection.GetConnection())
            {
                try
                {
                    log.Info("Retrieving the maximum number of participants for event ID: @eventID", eventID);
                    int maxAmount = conn.ExecuteScalar<int>("SELECT maxNumberOfCharacters FROM Event WHERE eventID = @EventID", new { EventID = eventID });

                    log.Info("Successfully retrieved maximum participants for event ID: @eventID", eventID);

                    if (ParticipantsInEvent(eventID) == maxAmount)
                    {
                        return true;
                    }
                    return false;
                } catch (SqlException ex)
                {
                    log.Trace("SQLException encountered while checking if the participant list is full.");
                    log.Error(ex, "Unable to check if the participant list is full.");
                    return false;
                }
            }
        }

        public bool MoveCharacterFromWaitingListToParticipantList(int eventID, string characterName)
        {

            if (!DBConnection.IsConnectionAvailable())
            {
                log.Error(exception: new TimeoutException(), "No connection to either the internet or the database available.");
                return false;
            }

            using (IDbConnection conn = DBConnection.GetConnection())
            {

                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        try
                        {
                            log.Info("Deleting character: @characterName from the participant list for event ID: @eventID", characterName, eventID);
                            //Delete character from Participants list
                            conn.Execute(@"DELETE FROM EventCharacter WHERE eventID = @EventID AND characterName = @CharacterName", new { EventID = eventID, CharacterName = characterName }, trans);
                            log.Info("Successfully deleted character: @characterName from the participant list for event ID: @eventID", characterName, eventID);
                        } catch (SqlException ex)
                        {
                            trans.Rollback();
                            log.Trace("SQLException encountered while deleting the character: @characterName from the participants list for event ID: @eventID.", characterName, eventID);
                            log.Error(ex, "Unable to delete the character from the participation list.");
                            return false;
                        }

                        try
                        {
                            log.Info("Retrieving the first character from the waiting list for event ID: @eventID", eventID);
                            //Select character from waiting list that signed up first
                            IEnumerable<EventCharacter> selectedCharacter = conn.Query<EventCharacter>(@"SELECT TOP 1 * FROM EventCharacterWaitingList WHERE eventID = @EventID ORDER BY signUpDateTime ASC",
                                new { EventID = eventID }, trans);
                            log.Info("Successfully retrieved the first character from the waiting list for event ID: @eventID", eventID);

                            try
                            {
                                log.Info("Deleting retrieved character from the waiting list for event ID: @eventID", eventID);
                                //Delete that character from WaitingList
                                conn.Execute(@"DELETE FROM EventCharacterWaitingList WHERE eventID = @EventID AND characterName = @CharacterName", selectedCharacter, trans);
                                log.Info("Successfully deleted character from the waiting list for event ID: @eventID", eventID);
                            } catch (SqlException ex)
                            {
                                trans.Rollback();
                                log.Trace("SQLException encountered while deleting the character: @characterName from the waiting list for event ID: @eventID.", characterName, eventID);
                                log.Error(ex, "Unable to delete the character from the waiting list.");
                                return false;
                            }

                            try
                            {
                                log.Info("Inserting retrieved character into the participation list for event ID: @eventID", eventID);
                                //Put that character in Participants List
                                conn.Execute(@"INSERT INTO EventCharacter (eventID, characterName, characterRole, signUpDateTime) " +
                                            "VALUES (@EventID, @CharacterName, @CharacterRole, @SignUpDateTime)", selectedCharacter, trans);
                                log.Info("Successfully inserted the retrieved character into the participation list for event ID: @eventID", eventID);
                            } catch (SqlException ex)
                            {
                                trans.Rollback();
                                log.Trace("SQLException encountered while inserting the retrieved character into the waiting list for event ID: @eventID.", eventID);
                                log.Error(ex, "Unable to insert the retrieved character into the participation list for event ID: @eventID.", eventID);
                                return false;
                            }
                        } catch (SqlException ex)
                        {
                            trans.Rollback();
                            log.Trace("SQLException encountered while retrieving the first character from the waiting list.");
                            log.Error(ex, "Unable to retrieve the first character from the waiting list.");
                            return false;
                        }

                        trans.Commit();
                        conn.Close();
                        return true;

                    } catch (Exception ex)
                    {
                        log.Trace("Unknown exception occured while attempting the transaction to move the character from the waiting list to the participant list.");
                        log.Error(ex, "Unknown exception occured while attempting the transaction to move the character from the waiting list to the participant list.");
                        trans.Rollback();
                        conn.Close();
                        return false;
                    }
                }
            }
        }

        public int ParticipantsInEvent(int eventID)
        {

            if (!DBConnection.IsConnectionAvailable())
            {
                log.Error(exception: new TimeoutException(), "No connection to either the internet or the database available.");
                return -1;
            }

            using (IDbConnection conn = DBConnection.GetConnection())
            {
                try
                {
                    log.Info("Retrieving the number of participants for event ID: @eventID", eventID);
                    int signedUpCount = conn.ExecuteScalar<int>("SELECT COUNT(*) FROM EventCharacter WHERE eventID = @EventID", new { EventID = eventID });
                    log.Info("Successfully retrieved the number of participants for event ID; @eventID", eventID);
                    return signedUpCount;
                } catch (SqlException ex)
                {
                    log.Trace("SQLException encountered while retrieving the number of participants in the event.");
                    log.Error(ex, "Unable to retrieve the number of participants in the event.");
                    return 0;
                }
            }
        }
    }
}
