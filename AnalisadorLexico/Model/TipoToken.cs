namespace AnalisadorLexico.Model
{
    public enum TipoToken
    {
        Undefined,
        String,
        Number,
        Eof,
        Space,
        Tab,
        LineBreak,
        RW_DO,
        RW_VAR,
        RW_Program,
        RW_Print,
        RW_Begin,
        RW_End,
        RW_Foward,
        RW_Repeat,
        RW_Turn,
        RW_Degrees,
        OP_Adicao,
        OP_Subtracao,
        OP_Multiplicacao,
        OP_Divisao
    }
}
