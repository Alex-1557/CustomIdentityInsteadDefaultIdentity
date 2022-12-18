import { HttpClientModule } from '@angular/common/http';
import { Injector, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule, Routes } from '@angular/router';
import { setAppInjector } from '@common/app-injector';
import { AppComponent } from '@components/app/app.component';
import { ConfirmModule } from '@components/modal/confirm.module';
import { API_BASE_URL } from '@services/ApiModule';
import { environment } from 'environments/environment';
import { ModalModule } from 'ngx-bootstrap/modal';
import { defaultSimpleModalOptions, SimpleModalModule } from 'ngx-simple-modal';
import { LayoutGlobalAdminModule } from './pages/admin/layoutGlobalAdmin.module';
import { LayoutAuthModule } from './pages/auth/layoutAuth.module';
import { LayoutClientModule } from './pages/client/layoutClient.module';

// import { ToastModule, ToastOptions } from 'ng2-toastr/ng2-toastr';

// let options: ToastOptions = new ToastOptions({
//    animate: 'flyRight',
//    positionClass: 'toast-bottom-right',
// });

const routes: Routes = [
    { path: '', redirectTo: 'home', pathMatch: 'full' },
    { path: 'restorePass/:token', redirectTo: 'auth/restorePass' },
    { path: '**', redirectTo: 'auth/login' },
];

@NgModule({
    bootstrap: [AppComponent],
    declarations: [AppComponent],
    imports: [
        RouterModule.forRoot(routes),
        BrowserModule,
        LayoutClientModule,
        LayoutAuthModule,
        LayoutGlobalAdminModule,
        ConfirmModule,
        HttpClientModule,
        SimpleModalModule.forRoot({ container: 'modal-container' }, {
            ...defaultSimpleModalOptions, ...{
                closeOnEscape: true,
                closeOnClickOutside: false,
                wrapperDefaultClasses: 'modal fade simpleModalBackdrop',
                wrapperClass: 'in',
                animationDuration: 100
            }
        }),
        ModalModule.forRoot(),
    ],
    providers: [
        ConfirmModule,
        { provide: API_BASE_URL, useValue: environment.apiUrl },
        { provide: 'windowObject', useValue: window },
    ]
})
export class AppModule {
    constructor(injector: Injector) {
        setAppInjector(injector);
    }
}

declare global {
    interface Array<T> {
        last(): T;
    }
}

Array.prototype.last = function () {
    const arr = this as Array<string>;
    return arr[arr.length - 1];
};
