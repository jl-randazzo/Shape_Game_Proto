using System;
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
        public int display_array_index { get; set; }
        public float time_elapsed { get; set; }
    }

    /// <summary>
    /// Editor variables
    /// </summary>
    public Sprite[] sprites;

    public GameObject sprite_empty;

    public GameObject dialog_sprite;

    [Tooltip("In letters per second, how quickly the characters are displayed in the dialogue box")]
    public float dialogue_speed;

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

        for (int i = 0; i < CHARACTER_CAPACITY; i++) {
            character_array[i] = GameObject.Instantiate(sprite_empty);
            character_array[i].transform.parent = dialog_sprite.transform;
        }

        DialogueInstructionElement element = DialogueInstructionElement.Text(statement);
        DialogueInstructionElement wait = DialogueInstructionElement.Wait(1);
        //active_instruction = new DialogueInstruction(new DialogueInstructionElement[] { wait, element }, true);
    }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        UpdateCharacterDialogueInstruction(Time.deltaTime);
    }


    private void UpdateCharacterDialogueInstruction(float delta_time) {
        if ((active_instruction == null && dialogue_instruction_queue.Count > 0)) {
            active_instruction = dialogue_instruction_queue.Dequeue();
            processing_data = new DialogueInstructionProcessingData();
        }

        processing_data.time_elapsed += delta_time;

        if (active_instruction != null) {
            DialogueInstructionElement active_element = active_instruction.Value.elements[processing_data.instruction_index];

            bool iterate = false;
            switch (active_element.type) {
                case DialogueInstructionElementType.CLEAR:
                    iterate = UpdateCharacterDialogueClearInstruction(active_element, ref delta_time);
                    break;
                case DialogueInstructionElementType.TEXT:
                    iterate = UpdateCharacterDialogueTextInstruction(active_element, ref delta_time);
                    break;
                case DialogueInstructionElementType.SET_SPEED:
                    iterate = UpdateCharacterDialogueSpeedInstruction(active_element, ref delta_time);
                    break;
                case DialogueInstructionElementType.WAIT:
                    iterate = UpdateCharacterDialogueWaitInstruction(active_element, ref delta_time);
                    break;
            }

            if (iterate == true) {
                processing_data.instruction_index++;
                processing_data.time_elapsed = 0;
                active_instruction = processing_data.instruction_index >= active_instruction.Value.elements.Length ? null : active_instruction;
                UpdateCharacterDialogueInstruction(delta_time);
            }
        }
    }
    private bool UpdateCharacterDialogueTextInstruction(DialogueInstructionElement word_element, ref float delta_time) {
        float elapsed_time = processing_data.time_elapsed - delta_time;
        float total_elapsed_time = processing_data.time_elapsed;
        processing_data.time_elapsed = total_elapsed_time;

        int low_index = (int)Mathf.Floor(elapsed_time * dialogue_speed);
        int high_index = (int)Mathf.Floor(total_elapsed_time * dialogue_speed);

        if (high_index == word_element.word.Length + 1) {
            delta_time = ((total_elapsed_time * dialogue_speed) - high_index) / dialogue_speed;
            return true;
        }

        float root_x = GlobalUtils.PixelsToFloat(8.5f * 7f);
        float y_offset = GlobalUtils.PixelsToFloat(1.1f * 11f);
        float root_y = y_offset - .55f;

        if (low_index != high_index && high_index > 0) {
            char c = word_element.word[high_index - 1];

            if (c < ' ' || c > 'z') {
                throw new System.Exception("Illegal character " + (char)c);
            }

            if (c != ' ') {
                c -= ' ';

                int display_index = processing_data.display_array_index;
                GameObject character_obj = character_array[processing_data.display_array_index];
                character_obj.SetActive(true);

                character_obj.GetComponent<SpriteRenderer>().sprite = sprites[c];
                int column = display_index % ROW_SIZE;
                int row = display_index / ROW_SIZE;
                character_obj.transform.position = new Vector3(-root_x + GlobalUtils.PixelsToFloat(7) * column, 0, root_y - y_offset * row);
            }

            processing_data.display_array_index++;
        }

        return false;
    }

    private bool UpdateCharacterDialogueClearInstruction(DialogueInstructionElement active_element, ref float delta_time) {
        for(int i = 0; i < character_array.Length; i++) {
            character_array[i].SetActive(false);
        }
        return true;
    }

    private bool UpdateCharacterDialogueSpeedInstruction(DialogueInstructionElement active_element, ref float delta_time) {
        dialogue_speed = active_element.speed;
        return true;
    }

    private bool UpdateCharacterDialogueWaitInstruction(DialogueInstructionElement active_element, ref float delta_time) {
        float elapsed_time = processing_data.time_elapsed - delta_time;
        float total_elapsed_time = processing_data.time_elapsed;
        processing_data.time_elapsed = total_elapsed_time;

        if (processing_data.time_elapsed >= active_element.wait_s) {
            delta_time -= active_element.wait_s;
            return true;
        }
        return false;
    }

}
