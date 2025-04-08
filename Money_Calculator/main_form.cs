using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Money_Calculator
{
    public partial class main_form : Form
    {
        

        public main_form()
        {
            InitializeComponent();
        }

        public class Exchange
        {
            public int money_first { get; set; }
            public int money_second { get; set; }
            public int mania_ratio { get; set; }
            public int bank_ratio { get; set; }

            public static int mania_high_commission_limit = 940000; // 최대 수수료 상한선
            public static int mania_high_commission = 47000;        // 최대 수수료 금액
            public static int mania_low_commission_limit = 20000;   // 최저 수수료 하한선
            public static int mania_low_commission = 1000;          // 최저 수수료 금액

            public static int mania_commission_percentage = 5;      // 통상 수수료 %
            public static int mania_cash_out_commission = 1000;     // 출금수수료 고정 1000원
            public long money_total { get; set; }
            public long mania_commission { get; set; }
            public long mania_result { get; set; }
            public long bank_result { get; set; }
            public Exchange()
            {
                this.money_first = 0;
                this.money_second = 0;
                this.mania_ratio = 0;
                this.bank_ratio = 0;
                this.money_total = 0;
                this.mania_commission = 0;
                this.mania_result = 0;
                this.bank_result = 0;
            }
            public void Calc(string mf, string ms, string mr, string br)
            {
                try
                {
                    if (int.TryParse(mf, out int int_mf) &&
                        int.TryParse(ms, out int int_ms) &&
                        int.TryParse(mr, out int int_mr) &&
                        int.TryParse(br, out int int_br))
                    {
                        this.money_first = int_mf;
                        this.money_second = int_ms;
                        this.mania_ratio = int_mr;
                        this.bank_ratio = int_br;
                        this.money_total = (long)(this.money_first * 10000 + this.money_second) / 100;

                        this.mania_result = (long)(money_total * this.mania_ratio);
                        this.bank_result = (long)(money_total * this.bank_ratio);

                        if (this.mania_result < Exchange.mania_low_commission_limit) // 총 판매금액 2만원 미만시 수수료 고정수수료 1000원 + 출금수수료 1000원
                        {
                            this.mania_commission = Exchange.mania_low_commission + Exchange.mania_cash_out_commission;
                        }
                        else if (this.mania_result > Exchange.mania_high_commission_limit) // 총 판매금액 94만원 이상시 고정수수료 47000원 + 출금수수료 1000원
                        {
                            this.mania_commission = Exchange.mania_high_commission + Exchange.mania_cash_out_commission;
                        }
                        else if (this.mania_result <= Exchange.mania_high_commission_limit &&
                                 this.mania_result >= Exchange.mania_low_commission_limit) // 총 판매금액 2만원 이상시 수수료 5% + 출금수수료 1000원
                        {
                            this.mania_commission = (long)(money_total * this.mania_ratio * Exchange.mania_commission_percentage / 100) + Exchange.mania_cash_out_commission;
                        }
                        this.mania_result = this.mania_result - this.mania_commission;
                    }
                    else
                    {
                        MessageBox.Show("잘못된 값이 입력 되었습니다.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("잘못된 값을 입력 했습니다.");
                }
            }
        }

        public class Sell
        {
            public long sell_price { get; set; }
            public int cost_price { get; set; }
            public int send_price { get; set; }
            public int commission_level { get; set; }
            public double commission_percentage { get; set; }
            public int commission { get; set; }
            public int total_get { get; set; }

            public static int normal_send_cost = 5000;
            public static int quick_send_cost = 20000;

            
            public Sell()
            {
                sell_price = 0;
                cost_price = 0;
                send_price = 0;
                commission_level = 0;
                commission_percentage = 0.0;
                commission = 0;
                total_get = 0;
            }

            public void commission_level_Setting(long sellprice)
            {
                if (sellprice < 100000) { this.commission_level = 0; }
                else if (sellprice >= 100000 && sellprice < 1000000) { this.commission_level = 1; }
                else if (sellprice >= 1000000 && sellprice < 5000000) { this.commission_level = 2; }
                else if (sellprice >= 5000000 && sellprice < 10000000) { this.commission_level = 3; }
                else if (sellprice >= 10000000 && sellprice < 25000000) { this.commission_level = 4; }
                else if (sellprice >= 25000000 && sellprice < 100000000) { this.commission_level = 5; }
                else if (sellprice >= 100000000) { this.commission_level = 6; }
            }
            public void Calc(long sellprice, int costprice, int selltype)
            {
                this.sell_price = sellprice;
                this.cost_price = costprice;

                

                switch (selltype) // 거래방식에 따라 다른 수수료 적용
                {
                    case 1: // 일반거래
                        {
                            this.send_price = 0;
                            if      (this.commission_level == 0) { this.commission_percentage = 0.0; }
                            else if (this.commission_level == 1) { this.commission_percentage = 0.8; }
                            else if (this.commission_level == 2) { this.commission_percentage = 1.8; }
                            else if (this.commission_level == 3) { this.commission_percentage = 3.0; }
                            else if (this.commission_level == 4) { this.commission_percentage = 4.0; }
                            else if (this.commission_level == 5) { this.commission_percentage = 5.0; }
                            else if (this.commission_level == 6) { this.commission_percentage = 6.0; }
                            break;
                        }
                    case 2: // 일반배송
                    case 3: // 특급배송
                        {
                            if      (selltype == 2) { this.send_price = 5000; }
                            else if (selltype == 3) { this.send_price = 20000; }

                            if      (this.commission_level == 0) { this.commission_percentage = 0.0; }
                            else if (this.commission_level == 1) { this.commission_percentage = 1.2; }
                            else if (this.commission_level == 2) { this.commission_percentage = 2.7; }
                            else if (this.commission_level == 3) { this.commission_percentage = 4.0; }
                            else if (this.commission_level == 4) { this.commission_percentage = 5.0; }
                            else if (this.commission_level == 5) { this.commission_percentage = 6.0; }
                            else if (this.commission_level == 6) { this.commission_percentage = 7.0; }
                            break;
                        }
                    default : { break; }
                }
                this.commission = (int)(this.sell_price * this.commission_percentage/100);
                this.total_get = (int)this.sell_price - this.commission - this.send_price - this.cost_price;

            }

        }

        public string mania_default = "4500";
        public string bank_default = "4000";

        Exchange ex = new Exchange();
        Sell sell = new Sell();

        private void main_form_Load(object sender, EventArgs e)
        {

        }

        private void calc_button_Click(object sender, EventArgs e)
        {
            if (main_tab.SelectedTab == main_tab_page1)
            {

                if (string.IsNullOrEmpty(money_first_box.Text))
                { money_first_box.Text = "0"; }
                if (string.IsNullOrEmpty(money_second_box.Text))
                { money_second_box.Text = "0"; }

                if (int.TryParse(money_first_box.Text, out int int_mf) &&
                    int.TryParse(money_second_box.Text, out int int_ms) &&
                    int.TryParse(mania_box.Text, out int int_mr) &&
                    int.TryParse(bank_box.Text, out int int_br))
                {
                    // 입력칸 예외 초기화 처리 (공백 or 음수)
                    if (int.Parse(money_first_box.Text) < 0)
                    { money_first_box.Text = "0"; }
                    if (int.Parse(money_second_box.Text) < 0)
                    { money_second_box.Text = "100"; }

                    if (int.Parse(money_first_box.Text) > 9999) // 9999억 초과시
                    { money_first_box.Text = "9999"; MessageBox.Show("9999억을 초과할 수 없습니다."); return; }
                    if (int.Parse(money_second_box.Text) > 9999) // 9999만 초과시
                    { money_second_box.Text = "9999"; MessageBox.Show("9999만을 초과할 수 없습니다."); return; }
                    if (string.IsNullOrEmpty(mania_box.Text) || int.Parse(mania_box.Text) < 0)
                    { mania_box.Text = mania_default; }
                    if (string.IsNullOrEmpty(bank_box.Text) || int.Parse(bank_box.Text) < 0)
                    { bank_box.Text = bank_default; }
                    if (int.Parse(money_first_box.Text) == 0 && int.Parse(money_second_box.Text) < 100)
                    { money_second_box.Text = "100"; MessageBox.Show("최소 금액은 100만 이상이여야 합니다."); return; }
                    // 최소 100만 이상 있어야하므로 99만 이하시 초기화

                    ex.Calc(money_first_box.Text, money_second_box.Text, mania_box.Text, bank_box.Text);
                    mania_result.ForeColor = Color.Blue;
                    bank_result.ForeColor = Color.Red;
                    if (ex.mania_result > ex.bank_result)
                    {
                        long gap = ex.mania_result - ex.bank_result;
                        string gap_string = gap.ToString("#,##0");
                        advise.ForeColor = Color.Blue;
                        advise.Text = $"아이템매니아 판매시 {gap_string}원 이득입니다.";
                    }
                    else if (ex.mania_result < ex.bank_result)
                    {
                        long gap = ex.bank_result - ex.mania_result;
                        string gap_string = gap.ToString("#,##0");
                        advise.ForeColor = Color.Red;
                        advise.Text = $"무통장거래 판매시 {gap_string}원 이득입니다.";
                    }
                    mania_result.Text = ex.mania_result.ToString("#,##0") + "원";
                    mania_commission.Text = ex.mania_commission.ToString("#,##0") + "원";
                    bank_result.Text = ex.bank_result.ToString("#,##0") + "원";
                    money_second_box.Focus();

                }
                else
                {
                    MessageBox.Show("잘못된 값을 입력 하였습니다.");
                }
            }
            else if(main_tab.SelectedTab == main_tab_page2)
            {
                if (string.IsNullOrEmpty(sell_price.Text)) { sell_price.Text = "0"; }
                if (string.IsNullOrEmpty(cost_price.Text)) { cost_price.Text = "0"; }
                
                if (long.TryParse(sell_price.Text, out long sellprice) && 
                    int.TryParse(cost_price.Text, out int costprice))
                {
                    int selltype = 0;
                    if (trade_radio.Checked) { selltype = 1; }
                    else if (send_radio.Checked) { selltype = 2; }
                    else if (quick_radio.Checked) { selltype = 3; }
                    sell.Calc(sellprice, costprice, selltype);
                    string result_string = sell.total_get.ToString("#,##0");

                    commission_label.Text = sell.commission.ToString("#,##0") + " 메소";
                    total_cost_label.Text = (sell.cost_price + sell.send_price).ToString("#,##0") + " 메소";
                    total_get_label.Text = sell.total_get.ToString("#,##0") + " 메소";
                    advise_2.ForeColor = Color.Blue;
                    advise_2.Text = $"판매시 {result_string} 메소 획득";
                    sell_price.Focus();
                }
                else
                {
                    MessageBox.Show("잘못된 값을 입력 하였습니다.");
                }

            }
        }
        private void reset_button_Click(object sender, EventArgs e)
        {
            if (main_tab.SelectedTab == main_tab_page1)
            {
                money_first_box.Text = "0";
                money_second_box.Text = "100";
                mania_box.Text = mania_default;
                bank_box.Text = bank_default;
                mania_result.Text = "0원";
                mania_commission.Text = "0원";
                bank_result.Text = "0원";
                advise.ForeColor = Color.Black;
                advise.Text = "값을 입력하고 계산버튼을 누르세요.";
            }
            else if(main_tab.SelectedTab == main_tab_page2)
            {
                sell_price.Text = "";
                cost_price.Text = "0";
                money_text_label.Text = "0메소";
                trade_radio.Checked = false;
                send_radio.Checked = true;
                quick_radio.Checked = false;
                commission_label_level10.ForeColor = Color.Black;
                commission_label_level100.ForeColor = Color.Black;
                commission_label_level500.ForeColor = Color.Black;
                commission_label_level1000.ForeColor = Color.Black;
                commission_label_level2500.ForeColor = Color.Black;
                commission_label_level10000.ForeColor = Color.Black;
                advise_2.ForeColor = Color.Black;
                advise_2.Text = "값을 입력하고 계산버튼을 누르세요.";
                sell_price.Focus();
            }
        }

        private void trade_radio_Checked(object sender, EventArgs e)
        {
            commission_label_level10.Text = "10만 메소 이상 : 0.8%";
            commission_label_level100.Text = "100만 메소 이상 : 1.8%";
            commission_label_level500.Text = "500만 메소 이상 : 3.0%";
            commission_label_level1000.Text = "1000만 메소 이상 : 4.0%";
            commission_label_level2500.Text = "2500만 메소 이상 : 5.0%";
            commission_label_level10000.Text = "1억 메소 이상 : 6.0%";
            send_cost_label.ForeColor = Color.Black;
            send_cost_label.Text = "배송비용 : 0 메소";
        }

        private void send_quick_radio_Checked(object sender, EventArgs e)
        {
            commission_label_level10.Text = "10만 메소 이상 : 1.2%";
            commission_label_level100.Text = "100만 메소 이상 : 2.7%";
            commission_label_level500.Text = "500만 메소 이상 : 4.0%";
            commission_label_level1000.Text = "1000만 메소 이상 : 5.0%";
            commission_label_level2500.Text = "2500만 메소 이상 : 6.0%";
            commission_label_level10000.Text = "1억 메소 이상 : 7.0%";
            if (send_radio.Checked)
            {
                send_cost_label.ForeColor = Color.Red;
                send_cost_label.Text = "배송비용 : 5000 메소";
            }
            else if (quick_radio.Checked)
            {
                send_cost_label.ForeColor = Color.Red;
                send_cost_label.Text = "배송비용 : 20000 메소";
            }
        }

        private void money_text_Change(object sender, EventArgs e)
        {
            if(long.TryParse(sell_price.Text, out long sellprice))
            {
                if(sellprice > int.MaxValue)
                {
                    sellprice = int.MaxValue;
                    sell_price.Text = int.MaxValue.ToString();
                }
                sell.commission_level_Setting(sellprice);
                int first = 0;
                int second = 0;
                int third = 0;
                for ( ; sellprice >= 100000000 ; )
                {
                    first += 1;
                    sellprice -= 100000000;
                }
                for( ; sellprice >= 10000 ; )
                {
                    second += 1;
                    sellprice -= 10000;
                }
                third = (int)sellprice;
                string text = "";
                if(first > 0) { text = text + $"{first}억 "; }
                if(second > 0){ text = text + $"{second}만 "; }
                if(third > 0) { text = text + $"{third}메소";}
                money_text_label.Text = text;
                
                commission_color_Change();
            }
        }

        private void commission_color_Change()
        {
            if (long.TryParse(sell_price.Text, out long sellprice))
            {
                commission_label_level10.ForeColor = Color.Black;
                commission_label_level100.ForeColor = Color.Black;
                commission_label_level500.ForeColor = Color.Black;
                commission_label_level1000.ForeColor = Color.Black;
                commission_label_level2500.ForeColor = Color.Black;
                commission_label_level10000.ForeColor = Color.Black;
            }
            if      (sell.commission_level == 1){commission_label_level10.ForeColor = Color.Red;}
            else if (sell.commission_level == 2){commission_label_level100.ForeColor = Color.Red;}
            else if (sell.commission_level == 3){commission_label_level500.ForeColor = Color.Red;}
            else if (sell.commission_level == 4){commission_label_level1000.ForeColor = Color.Red;}
            else if (sell.commission_level == 5){commission_label_level2500.ForeColor = Color.Red;}
            else if (sell.commission_level == 6){commission_label_level10000.ForeColor = Color.Red;}

        }

        
        
    }
}
