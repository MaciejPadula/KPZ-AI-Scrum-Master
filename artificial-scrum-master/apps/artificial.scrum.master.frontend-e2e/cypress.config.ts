import { nxE2EPreset } from '@nx/cypress/plugins/cypress-preset';

import { defineConfig } from 'cypress';

export default defineConfig({
  e2e: {
    ...nxE2EPreset(__filename, {
      cypressDir: 'src',
      webServerCommands: {
        default: 'nx run artificial.scrum.master.frontend:serve:development',
        production: 'nx run artificial.scrum.master.frontend:serve:production',
      },
      ciWebServerCommand:
        'nx run artificial.scrum.master.frontend:serve-static',
    }),
    baseUrl: 'http://localhost:4200',
  },
});
