using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusData
{
    public double m_nMoneyVal = 0;
    public double m_nSpeed = 6;
    public int m_nPotion = 1;
    public int m_nHP = 100;
    public double m_nMP = 100;
    public double m_ndecreaseMp = 0;
    public int m_nStr = 10;
    public int maxhp;
    public double maxmp;

    public StatusData(double money, double speed, int potion, int hp, double mp,double demp, int str)
    {
        this.m_nMoneyVal = money;
        this.m_nSpeed = speed;
        this.m_nPotion = potion;
        this.m_nHP = hp;
        this.m_nMP = mp;
        this.m_ndecreaseMp = demp;
        this.m_nStr = str;
        this.maxhp = m_nHP;
        this.maxmp = m_nMP;
}
}
