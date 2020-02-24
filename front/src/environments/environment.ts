// The file contents for the current environment will overwrite these during build.
// The build system defaults to the dev environment which uses `environment.ts`, but if you do
// `ng build --env=prod` then `environment.prod.ts` will be used instead.
// The list of which env maps to which file can be found in `angular-cli.json`.


//para Dev y pre: window.location.hostname:81
//para Prod: agilemeter:80

export const environment = {
  production: false,
  backendHost: "agilemeter-test-back.23.97.158.29.nip.io", 
  backendPort: 80,
};
