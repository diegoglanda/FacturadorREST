Public Class ModuloPrestacion
    Public Property Ambito As String
    Public Property CUITEmpresa As String
    Public Property CUITFinanciador As String
    Public Property CodigoPaciente As Long
    Public Property CodigoPlanFinanciador As String
    Public Property CodigoSede As String
    Public Property FechaOrden As Date
    Public Property CodigoCPT As List(Of String)
End Class

Public Class Modulo
    Public Property CodigoCPT As String
    Public Property DescripcionCPT As String
    Public Property CodigoModulo As String
    Public Property CodigoModuloHomologado As String
    Public Property CodigoPractica As String
    Public Property CodigoPracticaHomologado As String
    Public Property DescripcionModulo As String
    Public Property DescripcionPractica As String
    Public Property ImportePaciente As Double
    Public Property ImporteCobertura As Double
    Public Property MarcaPresupuesto As Boolean
    Public Property ModuloAutorizacion As Boolean
    Public Property ModuloConvenido As Boolean
    Public Property PracticaAutorizacion As Boolean
    Public Property PracticaConvenida As Boolean
    Public Property EstadoCPT As String
End Class

Public Class ModuloPrestacion_Respuesta
    Public Property CodigosCPTs As List(Of Modulo)
    Public Property EstadoRespuesta() As EstadoRespuesta

End Class
