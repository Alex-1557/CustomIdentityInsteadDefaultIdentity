import { Component, OnDestroy } from '@angular/core';
import { AuthService } from '@services/auth.service';
import { MenuService } from '@services/menu.service';
import { TranslationService } from '@services/translation.service';
import { appSettings } from './../../app.settings';


@Component({
    selector: 'layoutClient',
    templateUrl: './layoutClient.component.html',
    providers: [MenuService, TranslationService]
})
export class LayoutClientComponent implements OnDestroy {
    version = appSettings.version;
    constructor(private authService: AuthService,
        private menuService: MenuService) { }
    logout() {
        this.authService.logout();
    }

    public ngOnDestroy() {
        this.menuService.dispose();
    }
}
