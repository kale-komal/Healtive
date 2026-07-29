import Hero from "@/components/website/home/hero/Hero";
import WebsiteLayout from "../layouts/WebsiteLayout";
import WhyHealtive from "@/components/website/home/why-healtive/WhyHealtive";
import Modules from "@/components/website/home/Modules/Modules";
import Workflow from "@/components/website/home/Workflow/Workflow";


export default function Home() {
  return (
    <WebsiteLayout>
      <Hero/>
      <WhyHealtive/>
      <Modules />
      <Workflow />
    </WebsiteLayout>
  );
}