using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/Passive")]
public class Passive : ScriptableObject
{

    public string ItemCode; //�������ڵ�
    public SlotState ItemClass; //������ ����
    public string ItemName; // �������̸�
    public Sprite ImageCode; //�������� �̹���
    public Sprite ImageCode2; //�������� ������
    public int Description; // ���׷��̵� ������

    public enum SlotState
    {
        ELEMENTAL,
        ABILLTY,
        PASSIVE
    }
}
