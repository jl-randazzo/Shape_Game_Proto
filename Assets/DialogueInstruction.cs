
/// <summary>
/// Dilleneates the different types of dialogue instruction elements
/// </summary>
public enum DialogueInstructionElementType {
    WORD, SET_SPEED
}

/// <summary>
/// Basic structure type that is compiled at runtime and sent in a List to the DialogueController
/// </summary>
public struct DialogueInstructionElement {
    /// <summary>
    /// Common variables between all types
    /// </summary>
    public DialogueInstructionElementType type { get; private set; }

    public int start_index { get; private set; }

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
    /// Word static constructor
    /// </summary>
    /// <param name="word"></param>
    /// <returns></returns>
    public static DialogueInstructionElement Word(string word) {
        DialogueInstructionElement dialogueInstruction = new DialogueInstructionElement();
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
        dialogueInstruction.speed = speed;
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
