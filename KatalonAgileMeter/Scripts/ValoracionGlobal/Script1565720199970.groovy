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

'Pulsar sobre el botón "Acceder"'
WebUI.click(findTestObject('Login/input_Acceder'))

'Esperar a que cargue la pantalla'
WebUI.waitForPageLoad(20)

'Hacer click en el seleccionable equipo'
WebUI.click(findTestObject('Home/span_Equipo'))

'Esperar que se muestren las opciones de equipo'
WebUI.waitForElementPresent(findTestObject('Home/div_SelectorEquipo'), 5)

'Seleccionar la opción para el equipo'
WebUI.click(findTestObject('Home/div_SelectorEquipo'))

'Esperar que el botón "Evaluaciones Finalizadas" esté habilitado'
WebUI.waitForElementClickable(findTestObject('Home/a_EvaFinalizadas'), 5)

'Hacer click sobre el botón "Evaluaciones Finalizadas"'
WebUI.click(findTestObject('Home/a_EvaFinalizadas'))

'Esperar a que cargue la pantalla'
WebUI.waitForPageLoad(20)

WebUI.delay(5)

'Obtener el texto de la columna puntuación'
columnaPuntuacion = WebUI.getText(findTestObject('EvaluacionesFinalizadas/tbody_ValorColumna', [('columna') : GlobalVariable.colPuntuacion]))

'Pulsar en la columna resultados'
WebUI.click(findTestObject('EvaluacionesFinalizadas/button_ColResultados', [('columna') : GlobalVariable.colResultados]))

'Esperar a que cargue la pantalla'
WebUI.waitForPageLoad(20)

WebUI.delay(5)

'Obtener del valor de Valoración Global'
columnaValoracion = WebUI.getText(findTestObject('EvaluacionesResultados/tspan_ValoracionGlobal'))

'Añadir a la valoración global el caracter "%"'
columnaValoracion = (columnaValoracion + '%')

'Se verifican que los datos de la tabla y los resultados son los mismos'
WebUI.verifyEqual(columnaPuntuacion, columnaValoracion)

