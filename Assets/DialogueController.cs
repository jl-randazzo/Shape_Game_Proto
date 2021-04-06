using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DialogueController is called to show text on the screen
/// </summary>
public class DialogueController : MonoBehaviour {

    /// <summary>
    /// Private data structure used to track dialogue instruction processing state
    /// </summary>
    private struct DialogueInstructionProcessingData {
        public int instruction_index { get; set; }
        public float time_elapsed { get; set; }
    }

    /// <summary>
    /// Editor variables
    /// </summary>
    public Sprite[] sprites;

    public GameObject sprite_empty;

    public GameObject dialog_sprite;

    public string statement;

    /// <summary>
    /// private variables
    /// </summary>
    private Queue<DialogueInstruction> dialogue_instruction_queue = new Queue<DialogueInstruction>();

    private DialogueInstruction? active_instruction = null;

    private DialogueInstructionProcessingData processing_data;

    /// <summary>
    /// private 'constant' variables
    /// </summary>
    private GameObject[] character_array;

    private static readonly int CHARACTER_CAPACITY = 54;

    private static readonly int ROW_SIZE = 18;

    public void QueueDialogueInstruction(DialogueInstruction instruction) {
        dialogue_instruction_queue.Enqueue(instruction);
    }

    void Awake() {

        character_array = new GameObject[CHARACTER_CAPACITY];
        sprite_empty.SetActive(false);

        for(int i = 0; i < CHARACTER_CAPACITY; i++) {
            character_array[i] = GameObject.Instantiate(sprite_empty);
            character_array[i].transform.parent = dialog_sprite.transform;
        }

        float root_x = GlobalUtils.PixelsToFloat(8.5f * 7f);
        float y_offset = GlobalUtils.PixelsToFloat(1.1f * 11f);
        float root_y = y_offset -.55f;

        for (int i = 0; i < statement.Length; i++) {
            int c = statement[i];
            if (c < ' ' || c > 'z') {
                throw new System.Exception("Illegal character " + (char)c);
            } else if (c == ' ') {
                continue;
            }

            c -= ' ';

            character_array[i].SetActive(true);

            character_array[i].GetComponent<SpriteRenderer>().sprite = sprites[c];
            int column = i % ROW_SIZE;
            int row = i / ROW_SIZE;
            character_array[i].transform.position = new Vector3(-root_x + GlobalUtils.PixelsToFloat(7) * column, 0, root_y - y_offset * row);
        }
    }

    // Start is called before the first frame update
    void Start() {
        
        
    }

    // Update is called once per frame
    void Update() {
        if(active_instruction == null && dialogue_instruction_queue.Count > 0) {
            active_instruction = dialogue_instruction_queue.Dequeue();
            processing_data = new DialogueInstructionProcessingData();
        }

    }
}
