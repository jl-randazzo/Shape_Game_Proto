using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;

public class DialogueInstructionProvider : MonoBehaviour {
    public string[] instructions;

    public DialogueController controller;

    private DialogueInstruction[] runtime_instructions;

    private int instruction_index = 0;

    private const char SET_SPEED = 'S';
    
    private const char WAIT = 'W';

    void Awake() {
        runtime_instructions = instructions.Select(instruction => TransformInstruction(instruction)).ToArray();
    }

    private DialogueInstruction TransformInstruction(string instruction) {
        List<DialogueInstructionElement> element_list = new List<DialogueInstructionElement>();
        element_list.Add(DialogueInstructionElement.Clear());
        string original_instruction = instruction;

        while (instruction != null && instruction.IndexOf('{') != -1) {
            int index = instruction.IndexOf('{');

            if (index > 0) {
                string text = instruction.Substring(0, index);
                Debug.Log("Found text bit: " + text);
                element_list.Add(DialogueInstructionElement.Text(text));

                instruction = instruction.Substring(index);
            }

            index = instruction.IndexOf('}');
            if (index == -1) {
                throw new System.Exception("Closing character '}' not found in instruction: " + original_instruction);
            }

            element_list.Add(ProcessCommand(instruction.Substring(1, index - 1)));
            instruction = instruction.Length == index + 1 ? null : instruction.Substring(index + 1);
        }

        if (instruction != null) {
            element_list.Add(DialogueInstructionElement.Text(instruction));
        }


        return new DialogueInstruction(element_list.ToArray(), true);
    }

    private DialogueInstructionElement ProcessCommand(string command_string) {
        char command_type = command_string[0];
        switch (command_type) {
            case SET_SPEED:
                return ParseSpeedCommand(command_string);
            case WAIT:
                return ParseWaitCommand(command_string);
            default:
                throw new System.Exception("Unknown command type '" + command_type + "'");
        }
    }

    private DialogueInstructionElement ParseSpeedCommand(string command_string) {
        string[] args = command_string.Split(':', ',');
        return DialogueInstructionElement.Speed(Single.Parse(args[1]));
    }

    private DialogueInstructionElement ParseWaitCommand(string command_string) {
        string[] args = command_string.Split(':', ',');
        return DialogueInstructionElement.Wait(Single.Parse(args[1]));
    }

    // Start is called before the first frame update
    void Start() {
    }

    public void EmitDialogue() {
        controller.QueueDialogueInstruction(runtime_instructions[instruction_index++]);

        instruction_index = instruction_index == runtime_instructions.Length ? 0 : instruction_index;
    }
}
