/**
 * @license
 * Copyright Akveo. All Rights Reserved.
 * Licensed under the MIT License. See License.txt in the project root for license information.
 */
// The file contents for the current environment will overwrite these during build.
// The build system defaults to the dev environment which uses `environment.ts`, but if you do
// `ng build --env=prod` then `environment.prod.ts` will be used instead.
// The list of which env maps to which file can be found in `.angular-cli.json`.

export const environment = {
  production: false,
  webApiUrl: 'http://localhost:8004/api/',
  refreshRate: 5000,    //实时数据刷新频率，以毫秒为单位
  //webApiUrl: 'http://localhost:8003/api/',
  //webApiUrl: 'http://134.175.53.72:8080/api/',
};
