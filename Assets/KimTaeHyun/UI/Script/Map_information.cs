using UnityEngine;
using UnityEngine.UI;

public class Map_information : MonoBehaviour
{
    //맵 텍스트
    [SerializeField]
    private Text MapText;

    private void NewMethod(MapSpawn mapSpawn)
    {
        MapText.text = mapSpawn.Trains + " ";
    }
}
