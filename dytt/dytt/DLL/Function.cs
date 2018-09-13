using dyttspider.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThunderAgentLib;

namespace dyttspider.DLL
{
    /// <summary>
    /// 公共方法
    /// </summary>
    class Function
    {
        /// <summary>  
        /// 字符串转为UniCode码字符串  
        /// </summary>  
        /// <param name="s"></param>  
        /// <returns></returns>  
        public static string StringToUrlencode(string s)
        {
            StringBuilder sb = new StringBuilder();
            byte[] byStr = System.Text.Encoding.GetEncoding("GB2312").GetBytes(s); //默认是System.Text.Encoding.Default.GetBytes(str)
            for (int i = 0; i < byStr.Length; i++)
            {
                sb.Append(@"%" + Convert.ToString(byStr[i], 16));
            }

            return (sb.ToString());
        }
        /// <summary>
        /// 添加资源到迅雷
        /// </summary>
        /// <param name="src"></param>
        public static void AddTaskToThunder(MovieInfo info)
        {
            AgentClass agent = new AgentClass();
            
            agent.AddTask(info.Link,info.Title);
            agent.CommitTasks();
        }
    }
}
