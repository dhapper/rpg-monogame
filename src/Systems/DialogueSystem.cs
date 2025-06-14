using System;

public class DialogueSystem
{
    public string Line;
    public string[] Options;
    public int choice = 0;

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
        // var inputs = _inputSystem.GetInputState();
        // if (inputs.MoveDown)
        //     choice = choice < Options.Length - 1 ? ++choice : choice;
        // else if (inputs.MoveUp)
        //     choice = choice > 0 ? --choice : choice;
        // else if (inputs.Enter)
        //     ComputeChoice();
    }

    public void ManageInputs()
    {
        var inputs = InputSystem.GetInputState();
        if (inputs.MoveDown)
            choice = choice < Options.Length - 1 ? ++choice : choice;
        else if (inputs.MoveUp)
            choice = choice > 0 ? --choice : choice;
        else if (inputs.Enter)
            ComputeChoice();
    }

    public void ComputeChoice()
    {
        if (choice == 0)
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