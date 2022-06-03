namespace AnalisadorLexico.Model
{
    public enum TipoToken
    {
        Undefined,
        String,
        Eof,
        Space,
        Tab,
        LineBreak,
        RW_Program,
        RW_Print,
        RW_Begin,
        RW_End,
        RW_Foward,
        RW_Repeat,
        RW_Turn,
        OP_Adicao,
        OP_Subtracao,
        OP_Multiplicacao,
        OP_Divisao
    }
}
