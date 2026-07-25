import Header from "@/components/website/header/Header";
import Footer from "@/components/website/footer/Footer";

console.log(Header);
console.log(Footer);

export default function WebsiteLayout({ children }) {
  return (
    <>
      <Header />
      <main>{children}</main>
      <Footer />
    </>
  );
}