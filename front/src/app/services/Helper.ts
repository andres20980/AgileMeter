import { environment } from "environments/environment";

export class StaticHelper {
  static ReturnUrlByEnvironment() {
    let portText = ":" + environment.backendPort;
    return window.location.protocol +"//"+ environment.backendHost + portText + "/api/";
  }
}