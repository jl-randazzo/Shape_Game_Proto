
using System;
/// <summary>
/// Dilleneates the different types of dialogue instruction elements
/// </summary>
public enum DialogueInstructionElementType {
    TEXT, SET_SPEED, WAIT, CLEAR
}

/// <summary>
/// Basic structure type that is compiled at runtime and sent in a List to the DialogueController
/// </summary>
public struct DialogueInstructionElement {
    /// <summary>
    /// Common variables between all types
    /// </summary>
    public DialogueInstructionElementType type { get; private set; }

    /// <summary>
    /// Variables for 'WORD' type instructions
    /// </summary>
    public string word { get; private set; }

    /// <summary>
    /// Variables for 'SET_SPEED' type instructions
    /// </summary>
    public float speed { get; private set; }

    public bool passive { get; private set; }

    /// <summary>
    /// Variables for 'WAIT' type instructions
    /// </summary>
    public float wait_s { get; private set; }

    /// <summary>
    /// Word static constructor
    /// </summary>
    /// <param name="word"></param>
    /// <returns></returns>
    public static DialogueInstructionElement Text(string word) {
        DialogueInstructionElement dialogueInstruction = new DialogueInstructionElement();
        dialogueInstruction.type = DialogueInstructionElementType.TEXT;
        dialogueInstruction.word = word;
        return dialogueInstruction;
    }

    /// <summary>
    /// Speed static constructor
    /// </summary>
    /// <param name="speed"></param>
    /// <returns></returns>
    public static DialogueInstructionElement Speed(float speed) {
        DialogueInstructionElement dialogueInstruction = new DialogueInstructionElement();
        dialogueInstruction.type = DialogueInstructionElementType.SET_SPEED;
        dialogueInstruction.speed = speed;
        return dialogueInstruction;
    }

    /// <summary>
    /// Wait for an amount of time
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static DialogueInstructionElement Wait(float s) {
        DialogueInstructionElement dialogueInstruction = new DialogueInstructionElement();
        dialogueInstruction.type = DialogueInstructionElementType.WAIT;
        dialogueInstruction.wait_s = s;
        return dialogueInstruction;
    }

    /// <summary>
    /// Method that generates an instruction element to clear the dialogue box
    /// </summary>
    /// <returns></returns>
    public static DialogueInstructionElement Clear() {
        DialogueInstructionElement dialogueInstruction = new DialogueInstructionElement();
        dialogueInstruction.type = DialogueInstructionElementType.CLEAR;
        return dialogueInstruction;
    }
}

public enum DialogueInstructionType {
    CHARACTER_DIALOGUE
}

/// <summary>
/// DialogueInstruction encapsulates user control parameters and the corresponding elements for that dialogue instruction
/// </summary>
public struct DialogueInstruction {

    public DialogueInstructionType type { get; private set; }

    public DialogueInstructionElement[] elements { get; private set; }

    public bool passive { get; private set; }

    public DialogueInstruction(DialogueInstructionElement[] elements, bool passive) {
        type = DialogueInstructionType.CHARACTER_DIALOGUE;
        this.elements = elements;
        this.passive = passive;
    }
}
