Public Class EstadoRespuesta
    Private vCodigoRespuesta As String
    Private vMensaje As String

    Public Property CodigoRespuesta() As String
        '0: OK | 100:MENSAJE 1 | 200:MENSAJE 2
        Get
            Return vCodigoRespuesta
        End Get
        Set(ByVal value As String)
            vCodigoRespuesta = IIf(IsNothing(value), "", value)
        End Set
    End Property

    Public Property Mensaje() As String
        Get
            Return vMensaje
        End Get
        Set(ByVal value As String)
            vMensaje = IIf(IsNothing(value), "", value)
        End Set
    End Property
End Class
