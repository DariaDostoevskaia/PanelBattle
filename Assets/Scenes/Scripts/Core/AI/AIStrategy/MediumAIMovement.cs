using LegoBattaleRoyal.Panels.Models;
using LegoBattaleRoyal.Strategy;
using System;

public class MediumAIMovement : AIMovementStrategy
{
    public MediumAIMovement(int blocksToCapture, GridPosition currentPosition, PanelModel[] panelModels, Guid ownerId)
        : base(blocksToCapture, currentPosition, panelModels, ownerId)
    {
        ReturnWhenLosingCombatZone = true;
    }

    public override PanelModel Dicide()
    {
        if (TryUsePathfindingStrategy(out var panel))
            return panel;

        //TryUseToCaptureStrategy ���� 5������ �� ����� blocksToCapture - ����� config

        panel = UseRandomStrategy();
        return panel;
    }

}
