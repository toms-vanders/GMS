using GMS___Data_Access_Layer;


namespace GMS___Business_Layer
{
    public class EventCharacterWaitingListProcessor
    {
        private EventCharacterWaitingListAccess eventCharacterWaitingListAccess = new EventCharacterWaitingListAccess();

        public bool DeleteEventCharacterByEventIDAndCharacterName(int eventID, string characterName)
        {
            return eventCharacterWaitingListAccess.DeleteEventCharacterByEventIDAndCharacterName(eventID, characterName);
        }

        public bool MovePeopleFromWaitingList(int eventID)
        {
            EventCharacterAccess eca = new EventCharacterAccess();
            int PeopleToMove;
            int freeSpaces = eca.FreeSpacesInEvent(eventID);
            int peopleWaiting = eventCharacterWaitingListAccess.WaitingListLength(eventID);
            if (freeSpaces > peopleWaiting)
            {
                PeopleToMove = peopleWaiting;
            }
            else
            {
                PeopleToMove = freeSpaces;
            }
            return eventCharacterWaitingListAccess.MovePeopleFromWaitingList(eventID, PeopleToMove);
        }

    }
}
