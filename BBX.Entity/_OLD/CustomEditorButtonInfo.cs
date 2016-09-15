using System;

namespace BBX.Entity
{
    [Serializable]
    public class CustomEditorButtonInfo
    {
        
        private int m_id;
        public int Id { get { return m_id; } set { m_id = value; } }

        private int m_available;
        public int Available { get { return m_available; } set { m_available = value; } }

        private string m_tag;
        public string Tag { get { return m_tag; } set { m_tag = value; } }

        private string m_icon;
        public string Icon { get { return m_icon; } set { m_icon = value; } }

        private string m_replacement;
        public string Replacement { get { return m_replacement; } set { m_replacement = value; } }

        private string m_example;
        public string Example { get { return m_example; } set { m_example = value; } }

        private string m_explanation;
        public string Explanation { get { return m_explanation; } set { m_explanation = value; } }

        private int m_params;
        public int Params { get { return m_params; } set { m_params = value; } }

        private int m_nest;
        public int Nest { get { return m_nest; } set { m_nest = value; } }

        private string m_paramsdescript;
        public string Paramsdescript { get { return m_paramsdescript; } set { m_paramsdescript = value; } }

        private string m_paramsdefvalue;
        public string Paramsdefvalue { get { return m_paramsdefvalue; } set { m_paramsdefvalue = value; } }
    }
}