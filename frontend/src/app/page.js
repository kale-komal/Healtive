import Hero from "@/components/website/home/hero/Hero";
import WebsiteLayout from "../layouts/WebsiteLayout";
import WhyHealtive from "@/components/website/home/why-healtive/WhyHealtive";
import Modules from "@/components/website/home/Modules/Modules";
import Workflow from "@/components/website/home/Workflow/Workflow";
import ConnectedData from "@/components/website/home/ConnectedData/ConnectedData";
import FutureVision from "@/components/website/home/FutureVision/FutureVision";


export default function Home() {
  return (
    <WebsiteLayout>
      <Hero/>
      <WhyHealtive/>
      <Workflow />
      <Modules />     
      <ConnectedData />
      <FutureVision />
    </WebsiteLayout>
  );
}