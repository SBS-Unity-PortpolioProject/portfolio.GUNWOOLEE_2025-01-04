using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue /Dialogue Object")]
public class DialogueObject : ScriptableObject
{
    [SerializeField] [TextArea] private string [] dialogue;
    [SerializeField] private Response[] responsess;
    
    public string[] Dialogue => dialogue; 
    
    public Response[] Responses => responsess;
}
