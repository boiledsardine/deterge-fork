using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestData
{
    public QuestState state;
    public int questStepIndex;
    public QuestStepState[] questStepStates;
    public Vector3 playerPosition;

    public QuestData(QuestState state, int questStepIndex, QuestStepState[] questStepStates){
        this.state = state;
        this.questStepIndex = questStepIndex;
        this.questStepStates = questStepStates;
        this.playerPosition = new Vector3(0, 1.16f, 0);
    }
}
