using System;

public class DialogueSystem
{
    public string Line;
    public string[] Options;
    public int Choice = 0;

    // private InputSystem _inputSystem;
    private SleepSystem _sleepSystem;

    public DialogueSystem(SleepSystem sleepSystem)
    {
        _sleepSystem = sleepSystem;

        InitDialogue(
            "Sleep for the night?",
            [
                "Yes",
                "No"
            ]
        );
    }

    public void InitDialogue(string line, string[] options)
    {
        Line = line;
        Options = options;
    }

    public void Update()
    {
        ManageInputs();
    }

    public void ManageInputs()
    {
        var inputs = InputSystem.GetInputState();
        if (inputs.MoveDown)
            Choice = Choice < Options.Length - 1 ? ++Choice : Choice;
        else if (inputs.MoveUp)
            Choice = Choice > 0 ? --Choice : Choice;
        else if (inputs.Enter)
            ComputeChoice();
    }

    public void ComputeChoice()
    {
        if (Choice == 0)
        {
            // Sleep
            _sleepSystem.StartSleepCycle();
            GameStateManager.SetState(GameState.Playing);
        }
        else
        {
            // Default exit back to gameplay option
            GameStateManager.SetState(GameState.Playing);
        }
    }

}