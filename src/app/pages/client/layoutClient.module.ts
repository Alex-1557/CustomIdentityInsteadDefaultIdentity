import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SimulationModeModule } from '@components/simulationMode/simulationMode.module';
import { AuthGuard } from '@services/authGuard.service';
import { TranslationService } from '@services/translation.service';
import { ActiveLogComponent } from './activelog/activelog.component';
import { ActiveLogModule } from './activelog/activelog.module';
import { AllocationComponent } from './allocation/allocation.component';
import { AllocationModule } from './allocation/allocation.module';
import { AssignmentsComponent } from './assignments/assignments.component';
import { AssignmentsModule } from './assignments/assignments.module';
import { CustomersComponent } from './customers/customers.component';
import { CustomersModule } from './customers/customers.module';
import { DashboardComponent } from './dashboard/dashboard.component';
import { DashboardModule } from './dashboard/dashboard.module';
import { EquipmentComponent } from './equipment/equipment.component';
import { EquipmentModule } from './equipment/equipment.module';
import { HomeModule } from './home/home.module';
import { JobsComponent } from './jobs/jobs.component';
import { JobsModule } from './jobs/jobs.module';
import { LayoutClientComponent } from './layoutClient.component';
import { MaterialsComponent } from './materials/materials.component';
import { MaterialsModule } from './materials/materials.module';
import { NavMenuModule } from './navmenu/navmenu.module';
import { RentalsComponent } from './rentals/rentals.component';
import { RentalsModule } from './rentals/rentals.module';
import { ReportsComponent } from './reports/reports.component';
import { ReportsModule } from './reports/reports.module';
import { ShopComponent } from './shop/shop.component';
import { ShopModule } from './shop/shop.module';
import { ShopPurchaseOrdersModule } from './shopPurchaseOrders/shopPurchaseOrders.module';
import { ShopStockComponent } from './shopStock/shopStock.component';
import { ShopStockModule } from './shopStock/shopStock.module';
import { TeamComponent } from './teams/teams.component';
import { TeamModule } from './teams/teams.module';
import { UsersComponent } from './users/users.component';
import { UsersModule } from './users/users.module';

const layoutClientRoutes: Routes = [{
    path: '',
    component: LayoutClientComponent,
    canActivate: [AuthGuard],
    children: [
        { path: 'home', component: DashboardComponent, data: { title: 'Home' } },
        { path: 'equipment', component: EquipmentComponent, data: { title: 'Equipment' } },
        { path: 'users', component: UsersComponent, data: { title: 'Users' } },
        { path: 'materials', component: MaterialsComponent, data: { title: 'Materials' } },
        { path: 'jobs', component: JobsComponent, data: { title: 'Job' } },
        { path: 'teams', component: TeamComponent, data: { title: 'Team' } },
        { path: 'allocation', component: AllocationComponent, data: { title: 'Allocation' } },
        { path: 'reports', component: ReportsComponent, data: { title: 'Reports' } },
        { path: 'activelog', component: ActiveLogComponent, data: { title: 'Activity Log' } },
        { path: 'contacts', component: CustomersComponent, data: { title: 'Contacts' } },
        { path: 'rentals', component: RentalsComponent, data: { title: 'Rentals' } },
        { path: 'shop', component: ShopComponent, data: { title: 'Shop' } },
        { path: 'shop/id/:id', component: ShopComponent, data: { title: 'Shop' } },
        { path: 'shop/stock', component: ShopStockComponent, data: { title: 'Shop' } },
        {
            path: 'settings',
            loadChildren: () => import('./settings/settings.module')
                .then(m => m.SettingsModule),
        },
        {
            path: 'shop/purchaseOrders',
            loadChildren: () => import('./shopPurchaseOrders/shopPurchaseOrders.module')
                .then(m => m.ShopPurchaseOrdersModule),
        },
        { path: 'assignments', component: AssignmentsComponent, data: { title: 'Assignments' } }
    ]
}];

@NgModule({
    declarations: [
        LayoutClientComponent
    ],
    imports: [
        RouterModule.forChild(layoutClientRoutes),
        HomeModule,
        EquipmentModule,
        UsersModule,
        NavMenuModule,
        MaterialsModule,
        JobsModule,
        TeamModule,
        AllocationModule,
        SimulationModeModule,
        ReportsModule,
        ActiveLogModule,
        DashboardModule,
        CustomersModule,
        RentalsModule,
        ShopModule,
        ShopStockModule,
        ShopPurchaseOrdersModule,
        AssignmentsModule
    ],
    exports: [
        LayoutClientComponent
    ],
    providers: [TranslationService]
})

export class LayoutClientModule { }
