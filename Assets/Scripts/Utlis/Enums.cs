public enum e_MenuType
{
    None,
    Equip,      //���â
    Enhance,    //��ȭâ
    Quest,      //����Ʈâ
    Option,     //�ɼ�â
    Length
}

public enum e_StatType
{
    None,
    HP,                 //ü��
    MHP,                //�ִ�ü��
    Gauge,              //������
    MGauge,             //�ִ������
    Fill_Gauge,         //1��� ä��� ������
    Atk,                //���ݷ�
    Def,                //����
    Spd,                //�̵��ӵ�
    Acc,                //���߷�
    Eva,                //ȸ�Ƿ�
    Del,                //���ݵ�����
    Skill_Gauge,        //��ų�Ҹ������
    CoolTime,           //��Ÿ��
    Fill_HP,            //ȸ����
    Length
}

public enum e_Language
{
    Kor,        //�ѱ�
    Eng,        //����
    Jpn,        //�Ϻ���
    Length
}

public enum e_ItemType
{
    Spend,      //�Ҹ�ǰ
    Weapon,     //����
    Length
}

public enum e_EquipType
{
    Weapon,     //���⽽��
    Length
}

public enum e_Weapon
{
    None,
    One_handed_sword,       //�Ѽհ�
    Two_handed_sword,       //�μհ�
    Karate,                 //��Ŭ
    Glove,                  //�۷���
    Bag,                    //����
    Bead,                   //����
    Musical_instruments,    //�Ǳ�
    Dance,                  //������
    Length
}

public enum e_WeaponType
{
    Melee,          //��������
    Range,          //���Ÿ�����
    Length
}

public enum e_MonsterType
{
    Melee,      //����
    Range,      //���Ÿ�
    Boss,       //����
}

public enum e_Spend
{
    Bread,
    Croissant,
    Donut,
    HamburgerM,
    Pizza
}

public enum e_Scene
{
    None,
    Title,
    Loading,
    Dialog,
    School,
    Schoo_Building,
    School_KingSlime,
    Schoo_Building_1,
    School_ShiiDeathing,
    Length
}
public enum e_Bgm
{ 
    TitleSound,
    DialogSound,
    SchoolSound,
    FightSound,
    BossSound
}


public enum e_Sfx
{
    DanceSound,
    GuitarSound,
    SwoedSound,
    BulletSound,
    Hit,
    ExplosionSound = 8,
    BossAtteckSound,
    BossSkillSound,
    BossSkill2Sound,
    KingSlimeSkillSound,
    EnemyDie
}







