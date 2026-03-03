# SuperShop - Full-Stack E-Commerce System

### **Overview**
SuperShop is a professional full-stack e-commerce application built with **ASP.NET Core MVC 6.0**. The system is designed to provide a seamless shopping experience for customers while offering robust administrative controls for shop owners. It implements a complete business flow—from product discovery and stock-validated carting to order placement and administrative approval.

---

### **Key Modules & Features**

#### **1. Authentication & Role-Based Access**
- **Identity/Session-based Authentication:** Implemented secure login and registration system.
- **Role-based Redirection:** Admins are directed to the management dashboard, while Customers access the storefront.

#### **2. Dynamic Shopping Experience**
- **Advanced Filtering:** Users can search products by **Name** and **Price Range** simultaneously.
- **Category Navigation:** Products are organized by categories for better user experience.

#### **3. Smart Cart System (Buy Now Logic)**
- **Authentication Check:** Users are redirected to the login page if they attempt to buy without being signed in.
- **Stock Validation:** Real-time check on `ProductLimit`. Users cannot add out-of-stock items.
- **Quantity Management:** Adding the same product multiple times updates the quantity instead of duplicating rows.
- **Stock Synchronization:** The `ProductLimit` in the master table automatically decreases upon adding items to the cart.

#### **4. Order & Data Integrity (Snapshot Logic)**
- **Historical Accuracy:** During order placement, the system copies (snapshots) the product’s current name, image, and price into the `OrderDetails` table.
- **Resilience:** If an Admin deletes a product from the inventory, the customer's previous order history remains intact and accurate.

#### **5. Administrative Controls**
- **Full CRUD Operations:** Comprehensive management for Products and Categories.
- **Order Processing:** Admins can view detailed order logs, including customer info and product details.
- **Approval Workflow:** Once an admin accepts an order, data is moved to payment records, and the pending order is cleared.

---

### **Technical Stack**
* **Backend:** ASP.NET Core MVC 6.0, C#
* **Database:** MS SQL Server / MySQL (Entity Framework Core)
* **Frontend:** HTML5, CSS3, JavaScript, Razor Pages, Bootstrap
* **Design Pattern:** Repository-style query handling (AsQueryable)

---

### **How to Run This Project**
1. Clone the repository.
2. Update the connection string in `appsettings.json`.
3. Run `Update-Database` in the Package Manager Console.
4. Press `F5` or click 'Run' in Visual Studio.