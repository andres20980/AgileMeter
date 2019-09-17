import static com.kms.katalon.core.checkpoint.CheckpointFactory.findCheckpoint
import static com.kms.katalon.core.testcase.TestCaseFactory.findTestCase
import static com.kms.katalon.core.testdata.TestDataFactory.findTestData
import static com.kms.katalon.core.testobject.ObjectRepository.findTestObject
import com.kms.katalon.core.checkpoint.Checkpoint as Checkpoint
import com.kms.katalon.core.cucumber.keyword.CucumberBuiltinKeywords as CucumberKW
import com.kms.katalon.core.mobile.keyword.MobileBuiltInKeywords as Mobile
import com.kms.katalon.core.model.FailureHandling as FailureHandling
import com.kms.katalon.core.testcase.TestCase as TestCase
import com.kms.katalon.core.testdata.TestData as TestData
import com.kms.katalon.core.testobject.TestObject as TestObject
import com.kms.katalon.core.webservice.keyword.WSBuiltInKeywords as WS
import com.kms.katalon.core.webui.keyword.WebUiBuiltInKeywords as WebUI
import internal.GlobalVariable as GlobalVariable
import java.util.Date as Date
import java.text.SimpleDateFormat as SimpleDateFormat

'Abrir el navegador'
WebUI.openBrowser('')

'Maximizar el navegador'
WebUI.maximizeWindow()

'Navegar a la url del sistema'
WebUI.navigateToUrl(GlobalVariable.url)

'Informar el usuario'
WebUI.setText(findTestObject('Login/input_Usuario'), GlobalVariable.user)

'Informar la contraseña'
WebUI.setText(findTestObject('Login/input_Pass'), GlobalVariable.pass)

'Pulsar sobre el botón Acceder'
WebUI.click(findTestObject('Login/input_Acceder'))

'Esperar a que cargue la pantalla'
WebUI.waitForPageLoad(20)

'Hacer click en el seleccionable equipo'
WebUI.click(findTestObject('Home/span_Equipo'))

'Esperar que se muestren las opciones de equipo'
WebUI.waitForElementPresent(findTestObject('Home/div_SelectorEquipo'), 5)

'Seleccionar la opción para el equipo'
WebUI.click(findTestObject('Home/div_SelectorEquipo'))

'Hacer click en el seleccionable equipo'
WebUI.click(findTestObject('Home/span_Evaluacion'))

'Esperar que se muestren las opciones de evaluaciones'
WebUI.waitForElementPresent(findTestObject('Home/span_SelectorEvaluacion'), 5)

'Seleccionar el tipo de evaluación'
WebUI.click(findTestObject('Home/span_SelectorEvaluacion'))

'Esperar que el botón "Nueva evaluación" esté disponible'
WebUI.waitForElementClickable(findTestObject('Home/a_NuevaEvaluacion'), 5)

'Pulsar el botón "Nueva evaluación"'
WebUI.click(findTestObject('Home/a_NuevaEvaluacion'))

WebUI.waitForElementPresent(findTestObject('Home/button_Nueva'), 5, FailureHandling.OPTIONAL)

'Si hay evaluaciones pendientes, se mostrará un mensaje y se aceptará'
WebUI.click(findTestObject('Home/button_Nueva'), FailureHandling.OPTIONAL)

WebUI.delay(5)

'Esperar a que cargue la pantalla'
WebUI.waitForPageLoad(20)

'Volver al inicio'
WebUI.click(findTestObject('Home/a_Inicio'))

'Esperar a que cargue la pantalla'
WebUI.waitForPageLoad(20)

WebUI.delay(5)

'Esperar que el desplegable equipo esté disponible'
WebUI.waitForElementClickable(findTestObject('Home/span_Equipo'), 0)

'Hacer click en el seleccionable equipo'
WebUI.click(findTestObject('Home/span_Equipo'))

'Esperar que se muestren las opciones de equipo'
WebUI.waitForElementPresent(findTestObject('Home/div_SelectorEquipo'), 5)

'Seleccionar la opción para el equipo'
WebUI.click(findTestObject('Home/div_SelectorEquipo'))

'Esperar que el botón "Evaluaciones pendientes" esté disponible'
WebUI.waitForElementClickable(findTestObject('Home/a_EvaPendientes'), 5)

'Hacer click sobre el botón "Evaluaciones pendientes"'
WebUI.click(findTestObject('Home/a_EvaPendientes'))

'Esperar a que cargue la pantalla'
WebUI.waitForPageLoad(20)

WebUI.delay(5)

'Obtener el valor de fecha del primer registro'
valorFecha = WebUI.getText(findTestObject('EvaluacionesFinalizadas/tbody_ValorColumna', [('columna') : GlobalVariable.colFecha]))

'Obtener el valor de evaluación del primer registro'
valorEvaluacion = WebUI.getText(findTestObject('EvaluacionesFinalizadas/tbody_ValorColumna', [('columna') : GlobalVariable.colEvaluacion]))

'Obtener el valor de progreso del primer registro'
valorProgreso = WebUI.getText(findTestObject('EvaluacionesFinalizadas/tbody_ValorColumna', [('columna') : GlobalVariable.colProgreso]))

SimpleDateFormat dateFormat = new SimpleDateFormat('dd/MM/yyyy')

Date date = new Date()

'Recuperar la fecha del sistema en el formato dd/MM/yyyy'
String today = dateFormat.format(date)

'Verificar que la columna "Fecha" es igual a la fecha del sistema'
WebUI.verifyMatch(valorFecha, today, false, FailureHandling.CONTINUE_ON_FAILURE)

'Verificar que la columna "Evaluación" tiene el valor "SCRUM"'
WebUI.verifyEqual(valorEvaluacion, 'SCRUM', FailureHandling.CONTINUE_ON_FAILURE)

'Verificar que la columna "Progreso" está al 0%'
WebUI.verifyEqual(valorProgreso, '0 %', FailureHandling.CONTINUE_ON_FAILURE)

