using GMS___Data_Access_Layer;


namespace GMS___Business_Layer
{
    public class EventCharacterWaitingListProcessor : IEventCharacterWaitingListProcessor
    {
        private EventCharacterWaitingListAccess eventCharacterWaitingListAccess = new EventCharacterWaitingListAccess();

        public bool DeleteEventCharacterByEventIDAndCharacterName(int eventID, string characterName)
        {
            return eventCharacterWaitingListAccess.DeleteEventCharacterByEventIDAndCharacterName(eventID, characterName);
        }
    }
}
