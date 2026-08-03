import { Outfit, Noto_Sans } from "next/font/google";

import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap-icons/font/bootstrap-icons.css";
import BootstrapClient from "@/components/common/BootstrapClient";
import AOSProvider from "@/components/common/AOSProvider";
import "./globals.css";

import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { AuthProvider } from "@/contexts/AuthContext";

const outfit = Outfit({
  subsets: ["latin"],
  weight: ["400", "500", "600", "700"],
  variable: "--font-heading",
  display: "swap",
});

const notoSans = Noto_Sans({
  subsets: ["latin"],
  weight: ["300", "400", "500", "600", "700"],
  variable: "--font-body",
  display: "swap",
});

export const metadata = {
  title: "Healtive",
  description: "Hospital Management Software",
};

export default function RootLayout({ children }) {
  return (
    <html lang="en">
      <body className={`${outfit.variable} ${notoSans.variable}`}>

        <BootstrapClient />
        <AOSProvider />
        <AuthProvider>
          {children}
        </AuthProvider>
        <ToastContainer
          position="top-right"
          autoClose={3000}
          hideProgressBar={false}
          newestOnTop
          closeOnClick
          pauseOnHover
          theme="light"
        />
      </body>
    </html>
  );
}