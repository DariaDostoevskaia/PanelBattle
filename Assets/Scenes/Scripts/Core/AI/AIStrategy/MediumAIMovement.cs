using LegoBattaleRoyal.Core.AI.AIStrategy;
using LegoBattaleRoyal.Core.Panels.Models;
using System;

public class MediumAIMovement : AIMovementStrategy
{
    public int BlocksToCapture { get; }

    public MediumAIMovement(int blocksToCapture, GridPosition currentPosition, PanelModel[] panelModels, Guid ownerId)
        : base(blocksToCapture, currentPosition, panelModels, ownerId)
    {
        ReturnWhenLosingCombatZone = true;
        BlocksToCapture = blocksToCapture;
    }

    public override PanelModel Dicide()
    {
        if (TryUsePathfindingStrategy(out var panel))
            return panel;

        if (TryUseToCaptureStrategy(out panel))
            return panel;
        //TryUseToCaptureStrategy если 5блоков то домой blocksToCapture - через config

        panel = UseRandomStrategy();
        return panel;
    }

    private bool TryUseToCaptureStrategy(out PanelModel panelModel)
    {
        panelModel = null;
        var occupiedBlockCount = BlocksToCapture;
        foreach (var block in PanelModels)
        {
            if (block.IsOccupated(OwnerId))
            {
                occupiedBlockCount--;
            }
        }

        if (occupiedBlockCount == 0)
        {
            CreateNewPathToHome();
            return true;
        }
        return false;
    }
}