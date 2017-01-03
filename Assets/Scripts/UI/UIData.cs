using UnityEngine;
using System.Collections;

public class UIData {
    /// <summary>
    ///游戏相关设置
    /// </summary>

    public static int player_texture_int;//玩家自定义头像索引
    public static int player_name_int;//玩家昵称索引
    public static int gamehard_int;//游戏难度 
    public static int heroselectindex_int=3;//选中英雄索引

    public static string player_name;

    //游戏背景音乐的播放，包括UI背景音乐与游戏背景音乐
    public static bool game_backmusic_bool = true;
    public static bool game_effectmusic_bool = true;    //游戏音效
    public static bool game_help = true;//游戏帮助
    public static bool game_tip = true;//游戏提示
}
