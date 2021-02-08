using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



// ラジオボタンボタンのキー操作無効（その２）
class ExRadioButton : RadioButton
{
    protected override bool IsInputKey(Keys keyData)
    {
        return true;
    }

}