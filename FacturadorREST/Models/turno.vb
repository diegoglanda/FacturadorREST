Public Class turno
    Public Property Id As String
    Public Property Fecha As Date
    Public Property FechaInicio As Date
    Public Property FechaFin As Date
    Public Property SobreTurno As Boolean
    Public Property Medico As String
    Public Property Especialidad As String
    Public Property Servicio As String
    Public Property IdPaciente As Long
    Public Property Paciente As String
    Public Property Equipo As String
    Public Property Cobertura As String
    Public Property Ubicacion As String
    Public Property IdEstado As Integer
    Public Property EstadoTurno As String
    Public Property IdConsultorio As Integer
    Public Property Consultorio As String
    Public Property Llegada As Date
    Public Property IdProcedimiento As String
    Public Property Procedimiento As String
    Public Property CodigoAdmision As String
    Public Property CodigoSala As String
    Public Property CodigoConsultorio As String
    Public Property autoAdmision As Boolean
    Public Property Afiliado As String
    Public Property CUITFinanciador As String
    Public Property CodigoPaciente As Long
    Public Property CodigoPractica As String
    Public Property SolicitaCodigoSeguridad As Boolean
    Public Property TipoFormulario As String
    Public Property NumeroFormulario As String
    Public Property TipoPractica As String
    Public Property TipoTransaccion As String
End Class

Public Class turnoFiltro
    Public Property Id As Long
    Public Property Desde As Date
    Public Property Hasta As Date
End Class

Public Class turnoRecordatorioFiltro
    Public Property Id As Long
    Public Property fechaDesde As Date
    Public Property fechaHasta As Date
End Class

Public Class turnoRecordatorio
    Public Property tipoFormulario As String
    Public Property numeroFormulario As String
    Public Property paciente As String
    Public Property codigoPaciente As String
    Public Property numeroDocumento As String
    Public Property historiaClinica As String
    Public Property telefono1 As String
    Public Property telefono2 As String
    Public Property fechaTurno As Date
    Public Property codigoCpt As String
    Public Property descripcionCpt As String
    Public Property codigoPractica As String
    Public Property descripcionPractica As String
    Public Property codigoServicio As String
    Public Property descripcionServicio As String
    Public Property sede As String
    Public Property profesional As String
    Public Property pideConfirmacion As Boolean
    Public Property cuitCobertura As String
    Public Property descripcionCobertura As String
    Public Property razonSocialCobertura As String
    Public Property codigoPlan As String
    Public Property descripcionPlan As String
End Class

Public Class informarGestionTurno
    Public Property tipoFormulario As String
    Public Property numeroFormulario As String
    Public Property fechaEnvio As Date
    Public Property fechaGestion As Date
    Public Property resultadoGestion As String
    Public Property codigo As String
    Public Property observaciones As String
End Class

Public Class datosTurno
    Public Property tipoFormulario As String
    Public Property numeroFormulario As String
    Public Property pacienteNumero As String
    Public Property pacienteEstadoRecepcion As String
    Public Property pacienteRecepcion As String
    Public Property pacienteLlegada As Date
    Public Property pacienteConsultorio As Date
    Public Property pacienteSalida As Date
End Class

Public Class obtenerDatosTurnoFiltro
    Public Property codigoPaciente As String
    Public Property tipoFormulario As String
    Public Property numeroFormulario As String
End Class
