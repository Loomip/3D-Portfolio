public enum e_MenuType
{
    None,
    Equip,      //장비창
    Enhance,    //강화창
    Quest,      //퀘스트창
    Option,     //옵션창
    Length
}

public enum e_StatType
{
    None,
    HP,                 //체력
    MHP,                //최대체력
    Gauge,              //게이지
    MGauge,             //최대게이지
    Fill_Gauge,         //1대당 채우는 게이지
    Atk,                //공격력
    Def,                //방어력
    Spd,                //이동속도
    Acc,                //명중력
    Eva,                //회피력
    Del,                //공격딜레이
    Skill_Gauge,        //스킬소모게이지
    CoolTime,           //쿨타임
    Fill_HP,            //회복량
    Length
}

public enum e_Language
{
    Kor,        //한글
    Eng,        //영어
    Jpn,        //일본어
    Length
}

public enum e_ItemType
{
    Spend,      //소모품
    Weapon,     //무기
    Length
}

public enum e_EquipType
{
    Weapon,     //무기슬롯
    Length
}

public enum e_Weapon
{
    None,
    One_handed_sword,       //한손검
    Two_handed_sword,       //두손검
    Karate,                 //너클
    Glove,                  //글러브
    Bag,                    //가방
    Bead,                   //구슬
    Musical_instruments,    //악기
    Dance,                  //유니폼
    Length
}

public enum e_WeaponType
{
    Melee,          //근접공격
    Range,          //원거리공격
    Length
}

public enum e_MonsterType
{
    Melee,      //근접
    Range,      //원거리
    Boss,       //보스
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







