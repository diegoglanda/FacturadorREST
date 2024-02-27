Public Class FDH_VAL
    Private vFDH_VAL_IdPaciente As Long
    Private vFDH_VAL_CuitFinan As String
    Private vFDH_VAL_CodigoPlan As String
    Private vFDH_VAL_CondicionPac As String
    Private vFDH_VAL_Afiliado As String
    Private vFDH_VAL_TipoFormu As String
    Private vFDH_VAL_NumerFormu As String
    Private vFDH_VAL_FechaPractica As Date
    Private vFDH_VAL_AmbitoPractica As String
    Private vFDH_VAL_Empresa As String
    Private vFDH_VAL_Sede As String
    Private vFDH_VAL_Estudios As List(Of FDH_VAL_Estudio)

    Public Property FDH_VAL_IdPaciente() As Long
        Get
            Return vFDH_VAL_IdPaciente
        End Get
        Set(ByVal value As Long)
            vFDH_VAL_IdPaciente = IIf(IsNothing(value), 0, value)
        End Set
    End Property

    Public Property FDH_VAL_CuitFinan() As String
        Get
            Return vFDH_VAL_CuitFinan
        End Get
        Set(ByVal value As String)
            vFDH_VAL_CuitFinan = IIf(IsNothing(value), "", value)
        End Set
    End Property

    Public Property FDH_VAL_CodigoPlan() As String
        Get
            Return vFDH_VAL_CodigoPlan
        End Get
        Set(ByVal value As String)
            vFDH_VAL_CodigoPlan = IIf(IsNothing(value), 0, value)
        End Set
    End Property

    Public Property FDH_VAL_CondicionPac() As String
        Get
            Return vFDH_VAL_CondicionPac
        End Get
        Set(ByVal value As String)
            vFDH_VAL_CondicionPac = IIf(IsNothing(value), "", value)
        End Set
    End Property

    Public Property FDH_VAL_Afiliado() As String
        Get
            Return vFDH_VAL_Afiliado
        End Get
        Set(ByVal value As String)
            vFDH_VAL_Afiliado = IIf(IsNothing(value), "", value)
        End Set
    End Property

    Public Property FDH_VAL_TipoFormu() As String
        Get
            Return vFDH_VAL_TipoFormu
        End Get
        Set(ByVal value As String)
            vFDH_VAL_TipoFormu = IIf(IsNothing(value), "", value)
        End Set
    End Property

    Public Property FDH_VAL_NumerFormu() As String
        Get
            Return vFDH_VAL_NumerFormu
        End Get
        Set(ByVal value As String)
            vFDH_VAL_NumerFormu = IIf(IsNothing(value), "", value)
        End Set
    End Property

    Public Property FDH_VAL_FechaPractica() As Date
        Get
            Return vFDH_VAL_FechaPractica
        End Get
        Set(ByVal value As Date)
            vFDH_VAL_FechaPractica = IIf(IsNothing(value), "", value)
        End Set
    End Property

    Public Property FDH_VAL_AmbitoPractica() As String
        Get
            Return vFDH_VAL_AmbitoPractica
        End Get
        Set(ByVal value As String)
            vFDH_VAL_AmbitoPractica = IIf(IsNothing(value), "", value)
        End Set
    End Property

    Public Property FDH_VAL_Empresa() As String
        Get
            Return vFDH_VAL_Empresa
        End Get
        Set(ByVal value As String)
            vFDH_VAL_Empresa = IIf(IsNothing(value), "", value)
        End Set
    End Property

    Public Property FDH_VAL_Sede() As String
        Get
            Return vFDH_VAL_Sede
        End Get
        Set(ByVal value As String)
            vFDH_VAL_Sede = IIf(IsNothing(value), "", value)
        End Set
    End Property

    Public Property FDH_VAL_Estudios() As List(Of FDH_VAL_Estudio)
        Get
            Return vFDH_VAL_Estudios
        End Get
        Set(ByVal value As List(Of FDH_VAL_Estudio))
            vFDH_VAL_Estudios = IIf(IsNothing(value), Nothing, value)
        End Set
    End Property
End Class

Public Class FDH_VAL_Estudio
    Private vFDH_VAL_CodigoPractica As String
    Private vFDH_VAL_CantidadPractica As Integer
    Private vFDH_VAL_TipoPractica As String
    Private vFDH_VAL_CorrelativoPractica As Integer

    Public Property FDH_VAL_CodigoPractica() As String
        Get
            Return vFDH_VAL_CodigoPractica
        End Get
        Set(ByVal value As String)
            vFDH_VAL_CodigoPractica = IIf(IsNothing(value), "", value)
        End Set
    End Property

    Public Property FDH_VAL_CantidadPractica() As Integer
        Get
            Return vFDH_VAL_CantidadPractica
        End Get
        Set(ByVal value As Integer)
            vFDH_VAL_CantidadPractica = IIf(IsNothing(value), 0, value)
        End Set
    End Property
    Public Property FDH_VAL_TipoPractica() As String
        Get
            Return vFDH_VAL_TipoPractica
        End Get
        Set(ByVal value As String)
            vFDH_VAL_TipoPractica = IIf(IsNothing(value), "", value)
        End Set
    End Property
    Public Property FDH_VAL_CorrelativoPractica() As Integer
        Get
            Return vFDH_VAL_CorrelativoPractica
        End Get
        Set(ByVal value As Integer)
            vFDH_VAL_CorrelativoPractica = IIf(IsNothing(value), 0, value)
        End Set
    End Property

End Class

Public Class FDH_VAL_Respuesta
    Private vFDH_VAL_IdPaciente As Long
    Private vFDH_VAL_CuitFinan As String
    Private vFDH_VAL_CodigoPlan As String
    Private vFDH_VAL_CondicionPac As String
    Private vFDH_VAL_Afiliado As String
    Private vFDH_VAL_TipoFormu As String
    Private vFDH_VAL_NumerFormu As String
    Private vFDH_VAL_FechaPractica As Date
    Private vFDH_VAL_AmbitoPractica As String
    Private vFDH_VAL_Empresa As String
    Private vFDH_VAL_Sede As String
    Private vFDH_VAL_Estudios_Respuesta As List(Of FDH_VAL_Estudio_Respuesta)
    Private vEstadoRespuesta As EstadoRespuesta

    Public Property FDH_VAL_IdPaciente() As Long
        Get
            Return vFDH_VAL_IdPaciente
        End Get
        Set(ByVal value As Long)
            vFDH_VAL_IdPaciente = IIf(IsNothing(value), 0, value)
        End Set
    End Property

    Public Property FDH_VAL_CuitFinan() As String
        Get
            Return vFDH_VAL_CuitFinan
        End Get
        Set(ByVal value As String)
            vFDH_VAL_CuitFinan = IIf(IsNothing(value), "", value)
        End Set
    End Property

    Public Property FDH_VAL_CodigoPlan() As String
        Get
            Return vFDH_VAL_CodigoPlan
        End Get
        Set(ByVal value As String)
            vFDH_VAL_CodigoPlan = IIf(IsNothing(value), "", value)
        End Set
    End Property

    Public Property FDH_VAL_CondicionPac() As String
        Get
            Return vFDH_VAL_CondicionPac
        End Get
        Set(ByVal value As String)
            vFDH_VAL_CondicionPac = IIf(IsNothing(value), "", value)
        End Set
    End Property

    Public Property FDH_VAL_Afiliado() As String
        Get
            Return vFDH_VAL_Afiliado
        End Get
        Set(ByVal value As String)
            vFDH_VAL_Afiliado = IIf(IsNothing(value), "", value)
        End Set
    End Property

    Public Property FDH_VAL_TipoFormu() As String
        Get
            Return vFDH_VAL_TipoFormu
        End Get
        Set(ByVal value As String)
            vFDH_VAL_TipoFormu = IIf(IsNothing(value), "", value)
        End Set
    End Property

    Public Property FDH_VAL_NumerFormu() As String
        Get
            Return vFDH_VAL_NumerFormu
        End Get
        Set(ByVal value As String)
            vFDH_VAL_NumerFormu = IIf(IsNothing(value), "", value)
        End Set
    End Property

    Public Property FDH_VAL_FechaPractica() As Date
        Get
            Return vFDH_VAL_FechaPractica
        End Get
        Set(ByVal value As Date)
            vFDH_VAL_FechaPractica = IIf(IsNothing(value), "", value)
        End Set
    End Property

    Public Property FDH_VAL_AmbitoPractica() As String
        Get
            Return vFDH_VAL_AmbitoPractica
        End Get
        Set(ByVal value As String)
            vFDH_VAL_AmbitoPractica = IIf(IsNothing(value), "", value)
        End Set
    End Property

    Public Property FDH_VAL_Empresa() As String
        Get
            Return vFDH_VAL_Empresa
        End Get
        Set(ByVal value As String)
            vFDH_VAL_Empresa = IIf(IsNothing(value), "", value)
        End Set
    End Property

    Public Property FDH_VAL_Sede() As String
        Get
            Return vFDH_VAL_Sede
        End Get
        Set(ByVal value As String)
            vFDH_VAL_Sede = IIf(IsNothing(value), "", value)
        End Set
    End Property

    Public Property FDH_VAL_Estudios_Respuesta() As List(Of FDH_VAL_Estudio_Respuesta)
        Get
            Return vFDH_VAL_Estudios_Respuesta
        End Get
        Set(ByVal value As List(Of FDH_VAL_Estudio_Respuesta))
            vFDH_VAL_Estudios_Respuesta = IIf(IsNothing(value), Nothing, value)
        End Set
    End Property

    Public Property EstadoRespuesta() As EstadoRespuesta
        Get
            Return vEstadoRespuesta
        End Get
        Set(ByVal value As EstadoRespuesta)
            vEstadoRespuesta = IIf(IsNothing(value), Nothing, value)
        End Set
    End Property
End Class

Public Class FDH_VAL_Estudio_Respuesta
    Private vFDH_VAL_CodigoPractica As String
    Private vFDH_VAL_CantidadPractica As Integer
    Private vFDH_VAL_TipoPractica As String
    Private vFDH_VAL_CorrelativoPractica As Integer
    Private vFDH_VAL_MontoTarif As Double
    Private vFDH_VAL_MontoNetoFinan As Double
    Private vFDH_VAL_MontoIvaFinan As Double
    Private vFDH_VAL_MontoTotalFinan As Double
    Private vFDH_VAL_MontoNetoCopago As Double
    Private vFDH_VAL_MontoIvaCopago As Double
    Private vFDH_VAL_MontoTotalCopago As Double
    Private vFDH_VAL_MontoTotalBonoContribucion As Double
    Private vFDH_VAL_TipoMoneda As Integer
    Private vFDH_VAL_PorcentajePaciente As Double
    Private vFDH_VAL_PorcentajeCobertura As Double
    Private vFDH_VAL_RequiereAutorizacion As Boolean

    Public Property FDH_VAL_CodigoPractica() As String
        Get
            Return vFDH_VAL_CodigoPractica
        End Get
        Set(ByVal value As String)
            vFDH_VAL_CodigoPractica = IIf(IsNothing(value), "", value)
        End Set
    End Property

    Public Property FDH_VAL_CantidadPractica() As Integer
        Get
            Return vFDH_VAL_CantidadPractica
        End Get
        Set(ByVal value As Integer)
            vFDH_VAL_CantidadPractica = IIf(IsNothing(value), 0, value)
        End Set
    End Property

    Public Property FDH_VAL_TipoPractica() As String
        Get
            Return vFDH_VAL_TipoPractica
        End Get
        Set(ByVal value As String)
            vFDH_VAL_TipoPractica = IIf(IsNothing(value), "", value)
        End Set
    End Property

    Public Property FDH_VAL_CorrelativoPractica() As Integer
        Get
            Return vFDH_VAL_CorrelativoPractica
        End Get
        Set(ByVal value As Integer)
            vFDH_VAL_CorrelativoPractica = IIf(IsNothing(value), 0, value)
        End Set
    End Property

    Public Property FDH_VAL_MontoTarif() As Double
        Get
            Return vFDH_VAL_MontoTarif
        End Get
        Set(ByVal value As Double)
            vFDH_VAL_MontoTarif = IIf(IsNothing(value), 0, value)
        End Set
    End Property

    Public Property FDH_VAL_MontoNetoFinan() As Double
        Get
            Return vFDH_VAL_MontoNetoFinan
        End Get
        Set(ByVal value As Double)
            vFDH_VAL_MontoNetoFinan = IIf(IsNothing(value), 0, value)
        End Set
    End Property

    Public Property FDH_VAL_MontoIvaFinan() As Double
        Get
            Return vFDH_VAL_MontoIvaFinan
        End Get
        Set(ByVal value As Double)
            vFDH_VAL_MontoIvaFinan = IIf(IsNothing(value), 0, value)
        End Set
    End Property

    Public Property FDH_VAL_MontoTotalFinan() As Double
        Get
            Return vFDH_VAL_MontoTotalFinan
        End Get
        Set(ByVal value As Double)
            vFDH_VAL_MontoTotalFinan = IIf(IsNothing(value), 0, value)
        End Set
    End Property

    Public Property FDH_VAL_MontoNetoCopago() As Double
        Get
            Return vFDH_VAL_MontoNetoCopago
        End Get
        Set(ByVal value As Double)
            vFDH_VAL_MontoNetoCopago = IIf(IsNothing(value), 0, value)
        End Set
    End Property

    Public Property FDH_VAL_MontoIvaCopago() As Double
        Get
            Return vFDH_VAL_MontoIvaCopago
        End Get
        Set(ByVal value As Double)
            vFDH_VAL_MontoIvaCopago = IIf(IsNothing(value), 0, value)
        End Set
    End Property

    Public Property FDH_VAL_MontoTotalCopago() As Double
        Get
            Return vFDH_VAL_MontoTotalCopago
        End Get
        Set(ByVal value As Double)
            vFDH_VAL_MontoTotalCopago = IIf(IsNothing(value), 0, value)
        End Set
    End Property

    Public Property FDH_VAL_MontoTotalBonoContribucion() As Double
        Get
            Return vFDH_VAL_MontoTotalBonoContribucion
        End Get
        Set(ByVal value As Double)
            vFDH_VAL_MontoTotalBonoContribucion = IIf(IsNothing(value), 0, value)
        End Set
    End Property

    Public Property FDH_VAL_TipoMoneda() As Integer
        Get
            Return vFDH_VAL_TipoMoneda
        End Get
        Set(ByVal value As Integer)
            vFDH_VAL_TipoMoneda = IIf(IsNothing(value), 0, value)
        End Set
    End Property

    Public Property FDH_VAL_PorcentajePaciente() As Double
        Get
            Return vFDH_VAL_PorcentajePaciente
        End Get
        Set(ByVal value As Double)
            vFDH_VAL_PorcentajePaciente = IIf(IsNothing(value), 0, value)
        End Set
    End Property
    Public Property FDH_VAL_PorcentajeCobertura() As Double
        Get
            Return vFDH_VAL_PorcentajeCobertura
        End Get
        Set(ByVal value As Double)
            vFDH_VAL_PorcentajeCobertura = IIf(IsNothing(value), 0, value)
        End Set
    End Property
    Public Property FDH_VAL_RequiereAutorizacion() As Boolean
        Get
            Return vFDH_VAL_RequiereAutorizacion
        End Get
        Set(ByVal value As Boolean)
            vFDH_VAL_RequiereAutorizacion = IIf(IsNothing(value), False, value)
        End Set
    End Property
End Class
