using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    private void Start()
    {
        string str ="!#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[]^_`abcdefghijklmnopqrstuvwxyz{|}~" +
        "ｱｲｳｴｵｶｷｸｹｺｻｼｽｾｿﾀﾁﾂﾃﾄﾅﾆﾇﾈﾉﾊﾋﾌﾍﾎﾏﾐﾑﾒﾓﾔﾕﾖﾗﾘﾙﾚﾛﾜｦﾝｧｨｩｪｫｬｭｮｯｰﾞﾟ｡｢｣､･" +
        "ぁあぃいぅうぇえぉおかがきぎくぐけげこごさざしじすずせぜそぞただちぢっつづてでとどなにぬねのはばぱひびぴふぶぷへべぺほぼぽ" +
        "まみむめもゃやゅゆょよらりるれろゎわゐゑをん" +
        "ァアィイゥウェエォオカガキギクグケゲコゴサザシジスズセゼソゾタダチヂッツヅテデトドナニヌネノハバパヒビピフブプヘベペホボポ" +
        "マミムメモャヤュユョヨラリルレロヮワヰヱヲンヴヵヶ" +
        "ＡＢＣＤＥＦＧＨＩＪＫＬＭＮＯＰＱＲＳＴＵＶＷＸＹＺａｂｃｄｅｆｇｈｉｊｋｌｍｎｏｐｑｒｓｔｕｖｗｘｙｚ０１２３４５６７８９" +
        "負私絶対引俺繊細音、聞逃面倒手柔頼心安調ー旋律酔者黙耳汚混沌力無望前争見誰傷妹許助人頑張耐！？全震…"+
        "親政府官僚自身勤務目前所少女出会事目覚唯歌声扱族最後生残仇政府復讐誓道旅一行姉的存在怒怖歳見目実年齢" +
        "老見事酒飲酒場行大体旅出会正義熱血青年機械作得意辺知識豊富探元気娘弦実母子供扱怒調子者官僚の中出口数少喋出毒舌官僚" +
        "中地位立嫌謎多男実格何乗取妥協許正義忠実常自分考行動衰弱拾育上妹想姉運動大好付合大人好姉大好名事多本人嫌小時拾育無" +
        "口人命令従言事必聞思考理解不思議子政府主監獄管理拷問担当誰尊敬人間関係築事苦手仕事「力」政府中腕買政府官僚";


        string res = "";

        foreach(var c in str)
        {
            if(!res.Contains(c.ToString()))
            {
                res += c;
            }
            else
            {
                //Debug.Log(c);
            }
        }
        var l = res.ToCharArray().ToList();
        l.Sort();
        res = "";

        foreach(var c in l)
        {
            res += c;
        }


        Debug.Log(res);
    }
}
